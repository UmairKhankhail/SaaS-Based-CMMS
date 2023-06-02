using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Reflection.Metadata.Ecma335;

namespace MaintenanceWebApi.Models
{
    public class WorkRequest
    {
        [Key]
        public int wrAutoId { get; set; }

        public string username { get; set; }

        public string topName { get; set; }

        public string description { get; set; }

        public string approve { get; set; }

        public string companyId { get; set; }
    }
}
