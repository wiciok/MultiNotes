using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MultiNotes.Server
{
    public class LogWebApiRequestAttribute : ActionFilterAttribute
    {
        public LogWebApiRequestAttribute() { }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            LogManager.GetCurrentClassLogger().Info("\nRequest: \n" + actionExecutedContext.Request.ToString()+"\n\nResponse:\n "+actionExecutedContext.Response.ToString());
        }
    }
}