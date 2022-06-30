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

namespace FinalProject.WebMVC.Controllers
{
    public class BalanceController : Controller
    {
        // GET: Balance
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Get(int id)
        {
            if (ModelState.IsValid)
            {
                using (var facade = new BalanceFacade())
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
    }
}