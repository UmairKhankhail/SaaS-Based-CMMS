using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthenticationManager.Models
{
    public class AuthenticationRequest
    {
        public string uid { get; set; }
        public int uautoid { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string cid { get; set; }
        public List<string> list_permissions { get; set; }
    }
}
