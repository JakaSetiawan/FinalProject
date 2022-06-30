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
    [Table("TopUps")]
    public class TopUps
    {
        [Key]
        public int TopupID { get; set; }

        [Required, JsonConverter(typeof(DateTimeConverter))]
        public DateTime TopupDate { get; set; }

        [Required]
        public int AccountID { get; set; }

        [Required]
        public int BankID { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string ReferenceNumber { get; set; }

        public string TxnNumber { get; set; }

        [Required]
        public string Status { get; set; }

        [Required, JsonConverter(typeof(DateTimeConverter))]
        public DateTime CreatedDate { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? ProcessDate { get; set; }
    }

    public class TopUpsView
    {
        public int TopupID { get; set; }

        public DateTime TopupDate { get; set; }

        public int AccountID { get; set; }

        public int BankID { get; set; }
        public string BankName { get; set; }
        public decimal Amount { get; set; }

        public string ReferenceNumber { get; set; }

        public string TxnNumber { get; set; }

        public string Status { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime CreatedDate { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? ProcessDate { get; set; }
    }

}
