using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using InventoryWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Models
{
    public class Issuence
    {
        [Key]

        public int issuenceAutoId { get; set; }

        public string issuenceId { get; set; }

        public string status { get; set; }
        
        public string issuenceDescp { get; set; }
       
        public int userAutoId { get; set; }

        public string companyId { get; set; }


        [NotMapped]
        public int validityCheck { get; set; }

        [NotMapped]
        public List<IssuenceList> equipList { get; set; }


      
    }
}
