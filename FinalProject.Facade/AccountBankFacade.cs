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
    public class AccountBankFacade : BaseFacade<AccountBanks>
    {
        public async Task<PageResult> GetAccountBankPage(PageRequest option)
        {
            try
            {
                var sql = @"SELECT * FROM (select a.AccountID, a.BankID, b.BankName, a.Branch, a.AccountNumber from AccountBanks a 
                            JOIN Banks b ON a.BankID = b.BankID )x";
                var result = await Task.Run(() => {
                    var where = " 1=1 ";
                    var param = new object[option.Criteria.Count];
                    int i = 0;
                    option.Criteria.ForEach(c => {
                        where += string.Format("AND {0}.ToString().Contains(@{1}) ", c.criteria, i);
                        param[i] = c.value;
                        i++;
                    });

                    var query = Context.Database.SqlQuery<AccountBanksView>(sql).Where(where, param);
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
        
    }
    
}
