using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace AccountsWebApi.Models
{
    [Index(nameof(companyemail), IsUnique = true)]
    [Index(nameof(companyname), IsUnique = true)]
    public class Company
    {
        [Key]
        public string companyid { get; set; }

        [Required]
        public string companyname { get; set; }

        [Required]
        public string companyemail { get; set; }

        [Required]
        public string companyphone { get; set; }

        [Required]
        public string userfirstname { get; set; }

        [Required]
        public string userlastname { get; set; }

        [Required]
        public string password { get; set; }

        public string status { get; set; }
    }
}
