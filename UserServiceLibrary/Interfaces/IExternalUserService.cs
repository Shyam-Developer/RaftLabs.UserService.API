using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.Models;

namespace UserServiceLibrary.Interfaces
{
    public interface IExternalUserService
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}
