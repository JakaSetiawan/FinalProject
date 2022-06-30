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
    [Table("Merchants")]
    public class Merchants
    {
        [Key]
        public int MerchantID { get; set; }

        [Required]
        public string MerchantName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ContactName { get; set; }

        [Required]
        public string Address { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public int AccountID { get; set; }
    }
}
