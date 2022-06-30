using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CodeID.Helper;
using FinalProject.Entity;
using FinalProject.Facade;
using Newtonsoft.Json;
using FinalProject.WebMVC.Filter;
using FinalProject.Common;

namespace FinalProject.WebMVC.Controllers
{
    [@Authorize("Customer")]
    public class TopUpController : Controller
    {
        // GET: TopUp
        public ActionResult Index()
        {
            try
            {
                using (var facade = new BankFacade())
                {
                    var id = Convert.ToInt32(Session["UserID"]);
                    var bankid = facade.GetByAccountID(id);

                    if( id > 0 )
                    {
                        ViewBag.bankid = bankid;
                        ViewBag.ID = id;
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return View();
        }

        public ActionResult Bank()
        {
            return View();
        }

        public ActionResult Get(int id)
        {
            if (ModelState.IsValid)
            {
                using (var facade = new TopUpFacade())
                {
                    var cus = facade.GetByID(id);
                    if (cus == null)
                    {
                        return Json(ApiResponse.NotFound("Data not found"), JsonRequestBehavior.AllowGet);
                    }
                    return Json(ApiResponse.OK(cus), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(ApiResponse.BadRequest(id), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Paging(PageRequest page)
        {
            if (ModelState.IsValid)
            {
                page.Criteria.Add(new Criteria { criteria = "AccountID", value = Commons.GetUserID() });
                try
                {
                    using (var facade = new TopUpFacade())
                    {
                        var pgResult = await facade.GetPage(page);
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

        //paging by account id
        [HttpPost]
        public async Task<ActionResult> PagingByAccountID(PageRequest page)
        {
            if (ModelState.IsValid)
            {
                page.Criteria.Add(new Criteria { criteria = "AccountID", value = Commons.GetUserID() });
                try
                {
                    using (var facade = new AccountBankFacade())
                    {
                        var pgResult = await facade.GetPage(page);
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

        public async Task<ActionResult> Insert(TopUps topup)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var facade = new TopUpFacade())
                    {
                        var status = await facade.Insert(topup);
                        if (status)
                        {
                            return Json(ApiResponse.OK(status));
                        }
                        else
                        {
                            return Json(ApiResponse.NotAcceptable(topup));
                        }

                    }
                }
                catch (Exception ex)
                {
                    return Json(ApiResponse.InternalServerError(ex.Message));
                }
            }
            else
            {
                return Json(ApiResponse.BadRequest(topup));
            }
        }


    }
}