using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;

namespace MaintenanceWebApi.Models
{
    public class WorkOrder
    {
        [Key]
        public int woAutoId { get; set; }

        public string woId{ get; set; }

        public string woType{ get; set; }

        public string topName { get; set; }

        public int requestId { get; set; }

        public string assetDetials { get; set; }

        public string companyId { get; set; }
    }
}
