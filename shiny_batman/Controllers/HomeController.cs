using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace shiny_batman.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult Lista()
        {
            return PartialView();
        }

        public ActionResult Editar()
        {        
            return PartialView();
        }

        public ActionResult Visualizar()
        {
            return PartialView();
        }
    }
}
