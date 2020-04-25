using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SM.API.Models
{
    public class User
    {
        public String UserName { get; set; }
        public String UserRoles { get; set; }
        public String Auth_Token { get; set; }
    }
}
