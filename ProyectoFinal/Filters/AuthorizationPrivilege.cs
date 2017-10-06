using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ProyectoFinal.Utils;

namespace ProyectoFinal.Filters
{
    public class AuthorizationPrivilege : ActionFilterAttribute
    {
        public string Role { get; set; }
        public string OtherRole { get; set; }
        public const string USER = "UserName";
        public const string ROLE = "Role";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (string.IsNullOrEmpty(this.OtherRole))
            {
                this.OtherRole = string.Empty;
            }

            string userName = GetValue(context, USER);
            string userRole = GetValue(context, ROLE);
            var controllerRequested = context.RouteData.Values["controller"].ToString();
            RouteValueDictionary routeValueDictionaryForLogin = new RouteValueDictionary { { "controller", "Home" }, { "action", "Login" } }; //User not logged in
            RouteValueDictionary routeValueDictionaryForIndex = new RouteValueDictionary { { "controller", "Home" }, { "action", "Index" } }; //Logged user but different role. No permission
            context.HttpContext.Session.Add("ReturnURL", controllerRequested);

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userRole))
            {
                context.Result = new RedirectToRouteResult(routeValueDictionaryForLogin);
            }
            else if (userRole != this.Role && userRole != this.OtherRole)
            {
                context.Result = new RedirectToRouteResult(routeValueDictionaryForIndex);
            }
            
            base.OnActionExecuting(context);
        }

        private string GetValue(ActionExecutingContext context, string key)
        {
            return (context.HttpContext.Session[key] != null) ? context.HttpContext.Session[key].ToString() : string.Empty;
        }

    }
}