﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceWebApi.Models
{
    public class ProcedureMethod
    {
        [Key]
        public int pmAutoId { get; set; }

        public int pAutoId { get; set; }
        [ForeignKey("pAutoId")]
        public virtual Procedure Procedure { get; set; }

        public string pmName { get; set; }

        public string subItemPosition { get; set; }
        
        public string companyId { get; set; }

    }
}
