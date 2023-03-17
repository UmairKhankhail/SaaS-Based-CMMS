using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Userandprofile
    {
        [Key]
        public int userAndProfileAutoId { get; set; }

        public int profileAutoId { get; set; }
        [ForeignKey("profileAutoId")]
        public virtual Profile profiles { get; set; }

        public int userAutoId { get; set; }
        [ForeignKey("userAutoId")]
        public virtual User users { get; set; }

        public string companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company companies { get; set; }
    }
}
