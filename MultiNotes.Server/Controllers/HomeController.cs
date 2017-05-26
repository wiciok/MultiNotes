using System.Web;
using System.Web.Mvc;
using MultiNotes.Server.Services;

namespace MultiNotes.Server.Controllers
{
    [LogHttpRequest]
    public class HomeController : Controller
    {
        public string Index()
        {
            ViewBag.Title = "Home Page";
            return HttpUtility.HtmlEncode("This is MultiNotes application API service. You have no buisness here.");
        }
    }
}
