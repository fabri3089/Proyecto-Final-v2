using ProyectoFinal.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    [HandleError()]
    public class HelpController : Controller
    {
        // GET: Help
        [AuthorizationPrivilege(Role = "Admin")]
        public ActionResult Admins()
        {
            return View();
        }

        [AuthorizationPrivilege(Role = "Instructor")]
        public ActionResult Profesores()
        {
            return View();
        }

        

        [AuthorizationPrivilege(Role = "Client")]
        public ActionResult Socios()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }
               
    }
}