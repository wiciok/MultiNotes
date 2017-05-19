using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MultiNotes.Server
{
    public class LogHttpRequest : ActionFilterAttribute
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var sb = new StringBuilder();

            sb.Append(request.RequestType + " " + request.Url + "\n");

            var parsed = HttpUtility.ParseQueryString(request.Headers.ToString());

            foreach (string key in parsed)
            {
                sb.Append(key + ": " + parsed[key] + "\n");
            }

            var inputStream = request.InputStream;
            inputStream.Position = 0;

            using (var reader = new StreamReader(inputStream))
            {
                var body = reader.ReadToEnd();
                sb.Append("\n" + body);
            }

            _log.Log(LogLevel.Info, sb.ToString());

            base.OnActionExecuting(filterContext);
        }
    }
}