using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Facility
    {
        [Key]
        public int facilityautoid { get; set; }

        public string facilityid { get; set; }

        [Required]
        public string facilityname { get; set; }
        public string status { get; set; }

        public string companyid { get; set; }
        [ForeignKey("companyid")]
        public virtual Company companies { get; set; }
    }
}
