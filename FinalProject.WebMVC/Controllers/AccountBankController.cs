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

namespace FinalProject.WebMVC.Controllers
{
    [@Authorize]
    public class AccountBankController : Controller
    {
        // GET: AccountBank
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Customer()
        {
            var id = Convert.ToInt32(Session["UserID"]);
            ViewBag.ID = id;
            return View();
        }

        public ActionResult Get(int id)
        {
            if (ModelState.IsValid)
            {
                using (var facade = new AccountBankFacade())
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
        public async Task<ActionResult> PagingAccountBank(PageRequest page)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var facade = new AccountBankFacade())
                    {
                        var pgResult = await facade.GetAccountBankPage(page);
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

        public async Task<ActionResult> Insert(AccountBanks accbank)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var facade = new AccountBankFacade())
                    {
                        var status = await facade.Insert(accbank);
                        if (status)
                        {
                            return Json(ApiResponse.OK(status));
                        }
                        else
                        {
                            return Json(ApiResponse.NotAcceptable(accbank));
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
                return Json(ApiResponse.BadRequest(accbank));
            }
        }

        public async Task<ActionResult> Update(AccountBanks accbank)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var facade = new AccountBankFacade())
                    {
                        var status = await facade.Update(accbank);
                        if (status)
                        {
                            return Json(ApiResponse.OK(status));
                        }
                        else
                        {
                            return Json(ApiResponse.NotAcceptable(accbank));
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
                return Json(ApiResponse.BadRequest(accbank));
            }
        }

        public async Task<ActionResult> Delete(Int64 id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var facade = new AccountBankFacade())
                    {
                        var status = await facade.Delete(id);
                        if (status)
                        {
                            return Json(ApiResponse.OK(status));
                        }
                        else
                        {
                            return Json(ApiResponse.NotAcceptable("Delete data error!"));
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
                return Json(ApiResponse.BadRequest("Data not found"));
            }
        }
    }
}