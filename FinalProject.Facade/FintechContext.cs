using FinalProject.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Facade
{
    public class FintechContext : DbContext
    {
        public FintechContext():base("FINTECH_DB") { }

        public DbSet<Accounts> Accounts { get; set; }

        public DbSet<Merchants> Merchants { get; set; }

        public DbSet<Banks> Banks { get; set; }

        public DbSet<TopUps> TopUps { get; set; }
        
        public DbSet<AccountBanks> AccountBanks { get; set; }
        
        public DbSet<Balances> Balances { get; set; }

    }
}
