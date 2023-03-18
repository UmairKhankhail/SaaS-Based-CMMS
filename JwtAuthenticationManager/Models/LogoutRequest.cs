using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthenticationManager.Models
{
    public class LogoutRequest
    {
        public int userAutoId { get; set; }
        public string cId { get; set; }
        public string role { get; set; }
    }
}
