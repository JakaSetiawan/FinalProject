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
    [Table("Transactions")]
    public class Transactions
    {
        [Key]
        public int TransactionID { get; set; }

        [Required]
        public int AccountID { get; set; }
        
        [Required]
        public int MerchantID { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string ReferencesNo { get; set; }

        public string Remarks { get; set; }

        [Required]
        public string Status { get; set; }

        [Required,JsonConverter(typeof(DateTimeConverter))]
        public DateTime CreatedDate { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime ProcessDate { get; set; }
    }

    public class TransactionsView
    {
        public int TransactionID { get; set; }

        public int AccountID { get; set; }

        public int MerchantID { get; set; }

        public string MerchantName { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime TransactionDate { get; set; }

        public string Title { get; set; }

        public decimal Amount { get; set; }

        public string ReferencesNo { get; set; }

        public string Remarks { get; set; }

        public string Status { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime CreatedDate { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? ProcessDate { get; set; }
    }
}
