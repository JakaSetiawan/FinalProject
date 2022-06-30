using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using CodeID.Helper;
using FinalProject.Common;
using FinalProject.Entity;
using FinalProject.Facade;
using Newtonsoft.Json;
using FinalProject.WebMVC.Filter;

namespace FinalProject.WebMVC.Controllers
{
    public class IssueBookController : Controller
    {
        // GET: IssueBook
        public ActionResult Index()
        {
            ViewData["DateLength"] = ConfigurationManager.AppSettings["IssueLength"];
            ViewData["LateCharge"] = ConfigurationManager.AppSettings["LateCharge"];
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Paging(PageRequest page)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var facade = new IssueBookFacade())
                    {
                        var pgResult = await facade.GetIssueBookPage(page);
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

        public async Task<ActionResult> Insert(IssueBook issuebook)
        {
            Guid IDUser = new Guid("5B9AE28D-2C97-47B0-A636-06362AFA458D");
            issuebook.UserID = IDUser;
            issuebook.IssueBookID = Guid.NewGuid();
            if (ModelState.IsValid)
            {
                try
                {
                    using (var facade = new IssueBookFacade())
                    {
                        var status = await facade.proccessIssueBook(issuebook);
                        if (status)
                        {
                            return Json(ApiResponse.OK(status));
                        }
                        else
                        {
                            return Json(ApiResponse.NotAcceptable(issuebook));
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
                return Json(ApiResponse.BadRequest(issuebook));
            }
        }

        public async Task<ActionResult> ProccessReturnBook(ReturnBook returnbook)
        {
            returnbook.ReturnBookID = Guid.NewGuid();
            returnbook.CreatedBy = "admin.perpus";
            if(ModelState.IsValid)
            {
                try
                {
                    using (var facade = new ReturnBookFacade())
                    {
                        var status = await facade.ProccessReturn(returnbook);
                        if (status)
                        {
                            return Json(ApiResponse.OK(status));
                        }
                        else
                        {
                            return Json(ApiResponse.NotAcceptable(returnbook));
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
                return Json(ApiResponse.BadRequest(returnbook));
            }
        }
    }
}