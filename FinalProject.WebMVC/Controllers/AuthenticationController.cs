using CodeID.Helper;
using FinalProject.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.WebMVC.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Authentication
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UnAuthorize()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string userid, string password)
        {
            if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(password))
            {
                return Json(ApiResponse.BadRequest("Bad Request"));
            }

            return await Task.Run(() => {
                try
                {
                    int userID = Convert.ToInt32(userid);
                    using (var facade = new AccountFacade())
                    {
                        var account = facade.GetByID(userID);
                        if (account == null)
                        {
                            return Json(ApiResponse.NotFound("UserID is not registered!"));
                        }
                        var passw = facade.GetSaltPassword(password);
                        if (account.Password.Equals(passw))
                        {
                            Session["UserID"] = account.AccountID;
                            Session["UserName"] = account.FirstName + " " + account.LastName;
                            Session["Role"] = account.AccountType;
                            return Json(ApiResponse.OK(new
                            {
                                loggedIn = true,
                                message = "Login Success",
                                accountID = account.AccountID,
                                accountType = account.AccountType
                            }));
                        }
                        else
                        {
                            return Json(ApiResponse.NotFound("Invalid password!"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Json(ApiResponse.InternalServerError(ex.Message));
                }
            });
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return Redirect("/Authentication/Index");
        }
    }
}