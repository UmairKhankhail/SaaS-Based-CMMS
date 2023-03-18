using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthenticationManager.Models
{
    public class AuthenticationRequest
    {
        public string uId { get; set; }
        public int uAutoId { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string cId { get; set; }
        public List<string> listPermissions { get; set; }
    }
}
