using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Common
{

    public class Commons
    {
        public static string GetUserID()
        {
            var obj = System.Web.HttpContext.Current.Session["UserID"];
            if (obj is null)
            {
                return "";
            }
            return obj.ToString();
        }

        public static string GetRole()
        {
            var obj = System.Web.HttpContext.Current.Session["Role"];
            if (obj is null)
            {
                return "";
            }
            return obj.ToString();
        }
    }
   
}
