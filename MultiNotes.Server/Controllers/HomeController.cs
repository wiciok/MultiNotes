using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MultiNotes.Server.Controllers
{
    public class HomeController : Controller
    {
        public string Index()
        {
            WebApiApplication.GlobalLogger.Info("HomePage entered");

            ViewBag.Title = "Home Page";
            return HttpUtility.HtmlEncode("This is MultiNotes application API service. You have no buisness here.");
        }
    }
}
