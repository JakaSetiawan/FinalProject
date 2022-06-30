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
    public class TransactionFacade : BaseFacade<Transactions>
    {
        public async Task<PageResult> GetPagePending(PageRequest option)
        {
            try
            {
                var result = await Task.Run(() =>
                {
                    var where = " 1=1 ";
                    var param = new object[option.Criteria.Count];
                    int i = 0;
                    option.Criteria.ForEach(c =>
                    {
                        where += string.Format("AND {0}.ToString().Contains(@{1}) ", c.criteria, i);
                        param[i] = c.value;
                        i++;
                    });
                    var sql = @"SELECT * FROM (
                                    SELECT [TransactionID]
                                          ,a.[AccountID]
                                          ,a.[MerchantID]
	                                      ,MerchantName
                                          ,[TransactionDate]
                                          ,[Title]
                                          ,[Amount]
                                          ,[ReferencesNo]
                                          ,[Remarks]
                                          ,[Status]
                                          ,[CreatedDate]
                                          ,[ProcessDate]
                                      FROM [dbo].[Transactions] a JOIN Merchants b ON b.MerchantID = a.MerchantID)x";
                    var query = Context.Database.SqlQuery<TransactionsView>(sql).Where(where, param);
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
                var txn = GetByID(id);

                var merchant = Context.Merchants.SingleOrDefault(m => m.MerchantID == txn.MerchantID);
                if (merchant is null)
                {
                    throw new Exception("Merchant not found!");
                }
                var merchantBalance = Context.Balances.SingleOrDefault(a => a.AccountID == merchant.AccountID);
                var accountBalance = Context.Balances.SingleOrDefault(a => a.AccountID == txn.AccountID);
                if (accountBalance.Balance < txn.Amount)
                {
                    throw new Exception("Balance is not sufficient!");
                }
                merchantBalance.Balance += txn.Amount;
                accountBalance.Balance -= txn.Amount;
                merchantBalance.LastTransaction = DateTime.Now;
                accountBalance.LastTransaction = DateTime.Now;

                txn.Status = "ACCEPTED";
                txn.ProcessDate = DateTime.Now;
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
                var txn = GetByID(id);
                txn.Status = "REJECTED";
                return await Context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
