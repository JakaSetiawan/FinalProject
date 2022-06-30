using FinalProject.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Facade
{
    public class LibSysContext : DbContext
    {
        public LibSysContext() : base("LIB_SYSFO_DB") { }

        public DbSet<BookCategory> BookCategory { get; set; }

        public DbSet<Books> Books { get; set; }

        public DbSet<IssueBook> IssueBook { get; set; }

        public DbSet<ReturnBook> ReturnBook { get; set; }

        public DbSet<Students> Students { get; set; }

        public DbSet<Users> Users { get; set; }
    }
}
