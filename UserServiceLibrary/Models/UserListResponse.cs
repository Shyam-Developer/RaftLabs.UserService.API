using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Models
{
    public class UserListResponse
    {
        public int Page { get; set; }
        public int Total_Pages { get; set; }
        public List<User> Data { get; set; }
    }
}
