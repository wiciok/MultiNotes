using System.Web.Http.Filters;
using NLog;

namespace MultiNotes.Server.Services
{
    public class LogWebApiRequestAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            LogManager.GetCurrentClassLogger().Info("\nRequest: \n" + actionExecutedContext.Request+"\n\nResponse:\n "+actionExecutedContext.Response);
        }
    }
}