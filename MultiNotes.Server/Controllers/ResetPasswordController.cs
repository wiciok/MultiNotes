using System.Web;
using System.Web.Mvc;
using MultiNotes.Server.Services;

namespace MultiNotes.Server.Controllers
{
    [LogHttpRequest]
    public class ResetPasswordController : Controller
    {
        public string Index()
        {
            ViewBag.Title = "Home Page";
            return HttpUtility.HtmlEncode("This is MultiNotes application API service. You have no buisness here.");
        }

        [HttpGet]
        public string Reset(string token)
        {
            ViewBag.Title = "Password reset";
            if (string.IsNullOrEmpty(token))
            {
                return HttpUtility.HtmlEncode("This is MultiNotes application API service. You have no buisness here.");
            }
            else
            {
                string retVal;
                if ((retVal = PasswordResetService.Instance.ResetUserPassword(token)) == null)
                    return HttpUtility.HtmlEncode("Changing password error!");
                else
                    return HttpUtility.HtmlEncode("Password changed to: " + retVal + " . Please, save it, log in to application and change it manually!");
            }       
        }
    }
}