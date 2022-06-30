using FinalProject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Data.Entity;
using System.Threading.Tasks;
using CodeID.Helper;

namespace FinalProject.Facade
{
    public class BalanceFacade : BaseFacade<Balances>
    {
        public async Task<bool> Accepted(Transactions transaction, int id)
        {
            try
            {
                Merchants merchent = Context.Set<Merchants>().Find(transaction.MerchantID);
                Balances balances = GetByID(id);
                Balances balancesMerchant = GetByID(merchent.AccountID);
                if (balances == null)
                {
                    throw new Exception("User not found!");
                }
                if (balancesMerchant == null)
                {
                    throw new Exception("User not found!");
                }

                balances.Balance -= transaction.Amount;
                balances.LastTransaction = DateTime.Now;
                balancesMerchant.Balance += transaction.Amount;
                balancesMerchant.LastTransaction = DateTime.Now;
                transaction.Status = "ACCEPTED";
                transaction.ProcessDate = DateTime.Now;
                Context.Entry<Transactions>(transaction).State = EntityState.Modified;

                int affctd = await Context.SaveChangesAsync();
                return affctd > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> AcceptedTopup(TopUps topup, int id)
        {
            try
            {
                Banks bank = Context.Set<Banks>().Find(topup.BankID);
                Balances balances = GetByID(id);
                Balances balancesBank = GetByID(bank.AccountID);
                if (balances == null)
                {
                    throw new Exception("User not found!");
                }
                if (balancesBank == null)
                {
                    throw new Exception("User not found!");
                }

                balances.Balance += topup.Amount;
                balances.LastTransaction = DateTime.Now;
                balancesBank.Balance -= topup.Amount;
                balancesBank.LastTransaction = DateTime.Now;
                topup.Status = "ACCEPTED";
                topup.ProcessDate = DateTime.Now;
                Context.Entry<TopUps>(topup).State = EntityState.Modified;

                int affctd = await Context.SaveChangesAsync();
                return affctd > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
