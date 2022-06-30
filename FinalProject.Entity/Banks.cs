using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Entity
{
    [Table("Banks")]
    public class Banks
    {
        [Key]
        public int BankID { get; set; }

        [Required]
        public string BankName { get; set; }

        public string HeadOffice { get; set; }

        public string ContactName { get; set; }

        public string ContactPhone { get; set; }

        public string ContactEmail { get; set; }

        [Required]
        public int AccountID { get; set; }
    }
}
