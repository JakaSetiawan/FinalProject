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
    public class UsersController : Controller
    {
        // GET: Users
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
                    using (var facade = new UsersFacade())
                    {
                        //page.Order = "CreatedAt DESC";
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

        public async Task<ActionResult> Insert(Users user)
        {
            user.UserID = Guid.NewGuid();
            user.RegisterDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    using (var facade = new UsersFacade())
                    {
                        user.Password = facade.GetSaltPassword(user.Password);
                        var status = await facade.Insert(user);
                        if (status)
                        {
                            return Json(ApiResponse.OK(status));
                        }
                        else
                        {
                            return Json(ApiResponse.NotAcceptable(user));
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
                return Json(ApiResponse.BadRequest(user));
            }
        }

        public async Task<ActionResult> Update(Users user)
        {
            user.LastModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    using (var facade = new UsersFacade())
                    {
                        var status = await facade.Update(user);
                        if (status)
                        {
                            return Json(ApiResponse.OK(status));
                        }
                        else
                        {
                            return Json(ApiResponse.NotAcceptable(user));
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
                return Json(ApiResponse.BadRequest(user));
            }
        }

        public async Task<ActionResult> Delete(Guid id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var facade = new UsersFacade())
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