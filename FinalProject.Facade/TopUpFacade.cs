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
    public class TopUpFacade : BaseFacade<TopUps>
    {
        public async Task<PageResult> GetPagePending(PageRequest option)
        {
            try
            {
                var result = await Task.Run(() => {
                    var where = " 1=1 ";
                    var param = new object[option.Criteria.Count];
                    int i = 0;
                    option.Criteria.ForEach(c => {
                        where += string.Format("AND {0}.ToString().Contains(@{1}) ", c.criteria, i);
                        param[i] = c.value;
                        i++;
                    });
                    var sql = @"SELECT * FROM (SELECT [TopupID]
                              ,[TopupDate]
                              ,a.[AccountID]
                              ,a.[BankID]
	                          ,[BankName]
                              ,[Amount]
                              ,[ReferenceNumber]
                              ,[TxnNumber]
                              ,[Status]
                              ,[CreatedDate]
                              ,[ProcessDate]
                          FROM [dbo].[TopUps] a JOIN Banks b ON b.BankID = a.BankID )x";
                    var query = Context.Database.SqlQuery<TopUpsView>(sql).Where(where, param);
                    var count = query.Count();
                    var rows = query.OrderBy(option.Order)
                        .Skip((option.Page - 1) * option.PageSize)
                        .Take(option.PageSize)
                        .ToList();
                    var pageCount = (int)Math.Ceiling(count * 1.0 / option.PageSize);
                    var pageResult = new PageResult { Rows = rows, RowCount = count, PageCount = pageCount };
                    return pageResult;
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Accepted(int id)
        {
            try
            {
                var topup = GetByID(id);

                var bank = Context.Banks.SingleOrDefault(b => b.BankID == topup.BankID);
                if (bank is null)
                {
                    throw new Exception("Bank not found!");
                }
                var balanceBank = Context.Balances.SingleOrDefault(a => a.AccountID == bank.AccountID);
                var accountBalance = Context.Balances.SingleOrDefault(a => a.AccountID == topup.AccountID);
                if (balanceBank.Balance < topup.Amount)
                {
                    throw new Exception("Balance is not sufficient!");
                }
                balanceBank.Balance -= topup.Amount;
                accountBalance.Balance += topup.Amount;
                balanceBank.LastTransaction = DateTime.Now;
                accountBalance.LastTransaction = DateTime.Now;

                topup.Status = "ACCEPTED";
                topup.ProcessDate = DateTime.Now;
                return await Context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Rejected(int id)
        {
            try
            {
                var topup = GetByID(id);
                topup.Status = "REJECTED";
                topup.ProcessDate = DateTime.Now;
                return await Context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
