using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AccountsWebApi.Models
{
        [Index(nameof(username), IsUnique = true)]
        public class User
        {
            [Key]
            public int userautoid { get; set; }
            public string userid { get; set; }

            [Required]
            public string username { get; set; }
            public string password { get; set; }
            public string role { get; set; }
            public string status { get; set; }

            public int employeeautoid { get; set; }
            [ForeignKey("employeeautoid")]
            public virtual Employee employee { get; set; }

            public int deptautoid { get; set; }
            [ForeignKey("deptautoid")]
            public virtual Department department { get; set; }

            public string companyid { get; set; }
            [ForeignKey("companyid")]
            public virtual Company companies { get; set; }

            [NotMapped]
            public List<string> list_roles { get; set; }
    }
}
