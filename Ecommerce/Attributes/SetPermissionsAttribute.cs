using Ecommerce.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Attributes
{
    /// <summary>
    /// Custom authorization attribute for setting per-method accessibility 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SetPermissionsAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// The name of each action that must be permissible for this method, separated by a comma.
        /// </summary>
        public string Permissions { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool isUserAuthorized = base.AuthorizeCore(httpContext);
            if (!isUserAuthorized) return false;

            var user = SessionContext.GetUserData();
            if (user == null) return false;

            string[] permissions = Permissions.Split(',').ToArray();

            List<string> roles = user.Roles;

            foreach (var item in permissions)
                if (roles.Count((s) => s.ToLower() == item.ToLower()) > 0) return true;

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var user = SessionContext.GetUserData();

            if (user == null)
                base.HandleUnauthorizedRequest(filterContext);
            else
            {
                var Result = new ViewResult
                {
                    ViewName = "~/Views/Login/Index.cshtml",
                };
                Result.ViewBag.NotValidUser = "Non sei autorizzato a visualizzare la pagina richiesta. Effettua il login con un account autorizzato";

                filterContext.Result = Result;
            }
        }
    }
}