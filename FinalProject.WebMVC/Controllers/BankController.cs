using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CodeID.Helper;
using FinalProject.Common;
using FinalProject.Entity;
using FinalProject.Facade;
using Newtonsoft.Json;
using FinalProject.WebMVC.Filter;

namespace FinalProject.WebMVC.Controllers
{
    [@Authorize]
    public class BankController : Controller
    {
        // GET: Bank
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Get(int id)
        {
            if (ModelState.IsValid)
            {
                using (var facade = new BankFacade())
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
                try
                {
                    using (var facade = new BankFacade())
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

        public async Task<ActionResult> Insert(Banks bank)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var facade = new BankFacade())
                    {
                        var status = await facade.Insert(bank);
                        if (status)
                        {
                            return Json(ApiResponse.OK(status));
                        }
                        else
                        {
                            return Json(ApiResponse.NotAcceptable(bank));
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
                return Json(ApiResponse.BadRequest(bank));
            }
        }

        public async Task<ActionResult> Update(Banks bank)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var facade = new BankFacade())
                    {
                        var status = await facade.Update(bank);
                        if (status)
                        {
                            return Json(ApiResponse.OK(status));
                        }
                        else
                        {
                            return Json(ApiResponse.NotAcceptable(bank));
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
                return Json(ApiResponse.BadRequest(bank));
            }
        }

        public async Task<ActionResult> Delete(Int64 id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var facade = new BankFacade())
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