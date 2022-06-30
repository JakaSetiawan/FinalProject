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
    [Table("Balances")]
    public class Balances
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AccountID { get; set; }

        [Required]
        public decimal Balance { get; set; }

        [Required]
        public decimal RetainedBalance { get; set; }

        [Required, JsonConverter(typeof(DateTimeConverter))]
        public DateTime LastTransaction { get; set; }
    }
}
