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
    [Table("Books")]
    public class Books
    {
        [Key]
        public Guid? BookID { get; set; }

        [Required]
        public Guid BookCategoryID { get; set; }

        [Required]
        public string BookName { get; set; }

        public string Author { get; set; }

        public string Publisher { get; set; }

        public int Pages { get; set; }

        public string Isbn { get; set; }

        public int Stock { get; set; }

        public bool Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public string ModifiedBy { get; set; }
    }
}
