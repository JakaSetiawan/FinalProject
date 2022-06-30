using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
using FinalProject.Facade;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FinalProject.WebMVC.Filter
{
    public class AppAuthentication : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (filterContext.HttpContext.Session["UserName"] == null)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                    new
                    {
                        controller = "Authentication",
                        action = "Index",
                        returnUrl = filterContext.HttpContext.Request.Url.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped)
                    }
                    ));
            }
        }
    }

    public class Authorize : ActionFilterAttribute, IAuthorizationFilter
    {
        private string Role = "";
        public Authorize() { }

        public Authorize(string role)
        {
            Role = role;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var Session = filterContext.HttpContext.Session;
            var objUser = Session["UserID"];
            if (objUser == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                    new
                    {
                        controller = "Authentication",
                        action = "Index"
                    }
                    ));
            }
            var objRole = Session["Role"];
            string roleUser = objRole == null ? "" : objRole.ToString();
            string[] roles = Role.Split(',');
            if (!string.IsNullOrEmpty(Role) && !roles.Contains(roleUser))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                    new
                    {
                        controller = "Authentication",
                        action = "UnAuthorize"
                    }
                    ));
            }
        }
    }

    public class AppUserManager : UserManager<IdentityUser>
    {
        static FintechContext context = new FintechContext();
        static UserStore<IdentityUser> store = new UserStore<IdentityUser>(context);
        public AppUserManager() : base(store)
        {

        }

        public AppUserManager(IUserStore<IdentityUser> store) : base(store)
        {

        }


    }

}