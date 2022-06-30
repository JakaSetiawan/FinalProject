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
    [Table("Accounts")]
    public class Accounts
    {
        [Key]
        public int AccountID { get; set; }

        public string AccountType { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string PlaceOfBirth { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string MotherName { get; set; }

        [Required]
        public string IDNumber { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool Status { get; set; }

        [Required, JsonConverter(typeof(DateTimeConverter))]
        public DateTime RegisterDate { get; set; }

        [Required, JsonConverter(typeof(DateTimeConverter))]
        public DateTime LastModifiedDate { get; set; }

    }

    public class DateTimeConverter : IsoDateTimeConverter
    {
        public DateTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
