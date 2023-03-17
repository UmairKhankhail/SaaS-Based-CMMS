using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AccountsWebApi.Models
{
        [Index(nameof(userName), IsUnique = true)]
        public class User
        {
            [Key]
            public int userAutoId { get; set; }
            public string userId { get; set; }

            [Required]
            public string userName { get; set; }
            public string password { get; set; }
            public string role { get; set; }
            public string status { get; set; }

            public int employeeAutoId { get; set; }
            [ForeignKey("employeeAutoId")]
            public virtual Employee employee { get; set; }

            public int deptAutoId { get; set; }
            [ForeignKey("deptAutoId")]
            public virtual Department department { get; set; }

            public string companyId { get; set; }
            [ForeignKey("companyId")]
            public virtual Company companies { get; set; }

            [NotMapped]
            public List<string> listRoles { get; set; }
    }
}
