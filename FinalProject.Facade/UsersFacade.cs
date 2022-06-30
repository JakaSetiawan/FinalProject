using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.Entity;

namespace FinalProject.Facade
{
    public class UsersFacade : BasesFacade<Users>
    {
        public string GetSaltPassword(string password)
        {
            try
            {
                var sql = "SELECT [dbo].[SaltPassword](@0)";
                var param = new SqlParameter("@0", password);
                var passw = Context.Database.SqlQuery<string>(sql, param).SingleOrDefault();
                return passw;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
