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
    //[@Authorize]
    public class BookCategoryController : Controller
    {
        // GET: BookCategory
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Paging(PageRequest page)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var facade = new BookCategoryFacade())
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

        public async Task<ActionResult> Insert(BookCategory category)
        {
            category.BookCategoryID = Guid.NewGuid();
            category.CreatedBy = "admin.perpus";
            category.CreatedAt = DateTime.Now;
            if (ModelState.IsValid) 
            {
                try 
                {
                    using (var facade = new BookCategoryFacade())
                    {
                        var status = await facade.Insert(category);
                        if (status)
                        {
                            return Json(ApiResponse.OK(status));
                        }
                        else
                        {
                            return Json(ApiResponse.NotAcceptable(category));
                        }
                    }
                }
                catch(Exception ex)
                {
                    return Json(ApiResponse.InternalServerError(ex.Message));
                }
            }
            else
            {
                return Json(ApiResponse.BadRequest(category));
            }
        }

        public async Task<ActionResult> Update(BookCategory category)
        {
            category.ModifiedBy = "admin.perpus";
            category.ModifiedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    using (var facade = new BookCategoryFacade())
                    {
                        var status = await facade.Update(category);
                        if (status)
                        {
                            return Json(ApiResponse.OK(status));
                        }
                        else
                        {
                            return Json(ApiResponse.NotAcceptable(category));
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
                return Json(ApiResponse.BadRequest(category));
            }
        }

        public async Task<ActionResult> Delete(Guid id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var facade = new BookCategoryFacade())
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