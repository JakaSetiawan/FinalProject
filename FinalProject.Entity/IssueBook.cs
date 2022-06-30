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
    [Table("IssueBook")]
    public class IssueBook
    {
        [Key]
        public Guid? IssueBookID { get; set; }

        [Required]
        public Guid StudentID { get; set; }

        public Guid? UserID { get; set; }

        [Required]
        public Guid BookID { get; set; }

        public DateTime IssueDate { get; set; }

        [Required, JsonConverter(typeof(DateTimeConverter))]
        public DateTime ReturnDate { get; set; }

        public bool Status { get; set; }
    }

    public class IssueBookView
    {
        public Guid IssueBookID { get; set; }

        public string StudentName { get; set; }

        public string BookName { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public string AdminName { get; set; }

        public bool Status { get; set; }
    }
}
