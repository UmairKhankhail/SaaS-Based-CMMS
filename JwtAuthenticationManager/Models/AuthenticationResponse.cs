using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthenticationManager.Models
{
    public class AuthenticationResponse
    {
        public int uId { get; set; }
        public  string jwtToken { get; set; }
        public int expiresIn { get; set; }
    }
}
