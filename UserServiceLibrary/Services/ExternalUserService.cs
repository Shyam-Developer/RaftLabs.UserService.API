using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.Configuration;
using UserServiceLibrary.Interfaces;
using UserServiceLibrary.Models;
using Microsoft.Extensions.Options;

namespace UserServiceLibrary.Services
{
    public class ExternalUserService : IExternalUserService
    {
        #region Fields
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private ApiSettings settings;
        #endregion

        #region Constructor
        public ExternalUserService(HttpClient httpClient, IOptions<ApiSettings> options)
        {
            _httpClient = httpClient;
            _baseUrl = options.Value.BaseUrl.TrimEnd('/') + "/";
            this.settings = settings;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Fetches a user by their ID from the external API.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<User> GetUserByIdAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}users/{userId}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to fetch user. Status code: {response.StatusCode}");
                }

                var json = await response.Content.ReadFromJsonAsync<Dictionary<string, User>>();
                return json["data"];
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching user by ID", ex);
            }
        }

        /// <summary>
        /// Fetches all users from the external API, handling pagination.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = new List<User>();
            int page = 1;
            int totalPages = 1;

            try
            {
                do
                {
                    var response = await _httpClient.GetFromJsonAsync<UserListResponse>($"{_baseUrl}users?page={page}");

                    if (response?.Data != null)
                    {
                        users.AddRange(response.Data);
                        totalPages = response.Total_Pages;
                    }
                    else
                    {
                        break;
                    }

                    page++;
                }
                while (page <= totalPages);

                return users;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching all users", ex);
            }
        }
        #endregion
    }
}
