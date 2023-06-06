using System.ComponentModel.DataAnnotations;

namespace PreventiveMaintenanceWebApi.Models
{
    public class InspectionEntry
    {
        [Key]
        public int inspectionEntryAutoId { get; set; }
        public int assetModelId { get; set; }
        public string assetId { get; set; }
        public string question { get; set; }
        public string selectedOption { get; set; }
        public string remarks { get; set; }
        public string companyId { get; set; }
    }
}
