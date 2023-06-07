using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceWebApi.Models
{
    public class MethodSteps
    {
        [Key]
        public int msAutoId { get; set; }

        public int mtAutoId { get; set; }
        [ForeignKey("mtAutoId")]                              
        public virtual Method Method { get; set; }

        public string timeRequired { get; set; }

        public string toolRequired { get; set; }

        public string description { get; set; }

        public string companyId { get; set; }

    }
}
