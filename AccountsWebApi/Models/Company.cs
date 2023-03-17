using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace AccountsWebApi.Models
{
    [Index(nameof(companyEmail), IsUnique = true)]
    [Index(nameof(companyName), IsUnique = true)]
    public class Company
    {
        [Key]
        public string companyId { get; set; }

        [Required]
        public string companyName { get; set; }

        [Required]
        public string companyEmail { get; set; }

        [Required]
        public string companyPhone { get; set; }

        [Required]
        public string userFirstName { get; set; }

        [Required]
        public string userLastName { get; set; }

        [Required]
        public string password { get; set; }

        public string status { get; set; }
    }
}
