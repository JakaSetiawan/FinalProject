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
    [Table("AccountBanks")]
    public class AccountBanks
    {
        [Column(Order = 0), Key]
        public int AccountID { get; set; }

        [Column(Order = 1), Key]
        public int BankID { get; set; }

        [Required, MaxLength(20)]
        public string AccountNumber { get; set; }

        [Required, MaxLength(50)]
        public string Branch { get; set; }

        [Required, JsonConverter(typeof(DateTimeConverter))]
        public DateTime CreateDate { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? ModifiedDate { get; set; }
    }

    public class AccountBanksView
    {
        public int AccountID { get; set; }

        public int BankID { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }

        [Required, JsonConverter(typeof(DateTimeConverter))]
        public DateTime CreateDate { get; set; }

        [Required, MaxLength(20)]
        public string AccountNumber { get; set; }
    }
}
