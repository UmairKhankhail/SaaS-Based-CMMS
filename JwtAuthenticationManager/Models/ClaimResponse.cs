﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthenticationManager.Models
{
    public class ClaimResponse
    {
        public string uId { get; set; }
        public int uAutoId { get; set; }
        public List<string> role { get; set; }
        public string appRole { get; set; }
        public string companyId { get; set; }
        public bool isAuth { get; set; }
    }
}
