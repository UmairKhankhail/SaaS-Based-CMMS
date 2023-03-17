using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthenticationManager.Models
{
    public class ClaimRequest
    {
        public string token { get; set; }
        public string controllerActionName { get; set; }
    }
}
