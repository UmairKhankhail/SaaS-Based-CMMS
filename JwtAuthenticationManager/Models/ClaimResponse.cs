using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthenticationManager.Models
{
    public class ClaimResponse
    {
        public string uid { get; set; }
        public int uautoid { get; set; }
        public List<string> role { get; set; }
        public string approle { get; set; }
        public string companyid { get; set; }
        public bool isauth { get; set; }
    }
}
