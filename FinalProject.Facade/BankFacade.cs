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
    public class BankFacade : BaseFacade<Banks>
    {
        public Banks GetByAccountID(int id)
        {
            try
            {
                return Context.Banks.SingleOrDefault(b => b.AccountID == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
