using System.ComponentModel.DataAnnotations;

namespace MaintenanceWebApi.Models
{
    public class Procedure
    {
        [Key]
        public int pAutoId { get; set; }

        public string pName { get; set; }

        public string tom { get; set; }

        public string assetName{ get; set; }

        public string companyId { get; set; }

    }
}
