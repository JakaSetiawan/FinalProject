using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CodeID.Helper;
using FinalProject.Common;
using FinalProject.Facade;
using FinalProject.WebMVC.Filter;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace FinalProject.WebMVC.Controllers
{
    [@Authorize]
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            try
            {
                using (var facade = new BalanceFacade())
                {
                    var id = Convert.ToInt32(Session["UserID"]);
                    var balance = facade.GetByID(id);
                    if (!(balance == null))
                    {
                        ViewBag.Balance = balance;
                    }
                    using (var accFacade = new AccountFacade())
                    {
                        var account = accFacade.GetByID(id);
                        ViewBag.Account = account;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PendingTopup(PageRequest page)
        {
            if (ModelState.IsValid)
            {
                var bankid = "-1";
                try
                {
                    using (var facade = new TopUpFacade())
                    {
                        var id = Convert.ToInt32(Commons.GetUserID());
                        var account = new AccountFacade().GetByID(id);
                        if (account.AccountType.Equals("Bank"))
                        {
                            var bank = new BankFacade().GetByAccountID(id);
                            bankid = bank.BankID + "";
                        }
                        page.Criteria.Add(new Criteria { criteria = "BankID", value = bankid });
                        page.Criteria.Add(new Criteria { criteria = "Status", value = "PENDING" });
                        var pgResult = await facade.GetPagePending(page);
                        var result = JsonConvert.SerializeObject(ApiResponse.OK(pgResult));
                        return Content(result, "application/json");
                    }
                }
                catch (Exception ex)
                {
                    return Json(ApiResponse.InternalServerError(ex.Message));
                }
            }
            else
            {
                return Json(ApiResponse.BadRequest(page));
            }
        }

        public async Task<ActionResult> PendingTxn(PageRequest page)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var facade = new TransactionFacade())
                    {
                        page.Criteria.Add(new Criteria { criteria = "AccountID", value = Commons.GetUserID() });
                        page.Criteria.Add(new Criteria { criteria = "Status", value = "PENDING" });
                        var pgResult = await facade.GetPagePending(page);
                        var result = JsonConvert.SerializeObject(ApiResponse.OK(pgResult));
                        return Content(result, "application/json");
                    }
                } 
                catch (Exception ex)
                {
                    return Json(ApiResponse.InternalServerError(ex.Message));
                }
            }
            else
            {
                return Json(ApiResponse.BadRequest(page));
            }
        }
    }
}