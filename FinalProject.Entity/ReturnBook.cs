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
    [Table("ReturnBook")]
    public class ReturnBook
    {
        [Key]
        public Guid? ReturnBookID { get; set; }

        [Required]
        public Guid IssueBookID { get; set; }

        public DateTime ReturnDate { get; set; }

        public int DayElaps { get; set; }

        public bool IsFine { get; set; }

        public string Description { get; set; }

        public string CreatedBy { get; set; }

    }
}
