using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MongoDB.Bson;
using MultiNotes.Server.Services;

namespace MultiNotes.Server.Controllers
{
    //class returning bsonId, used for creating new notes or users in client apps
    [LogWebApiRequest]
    [RoutePrefix("api/id")]
    public class IdController : ApiController
    {
        [ResponseType(typeof(string))]
        public HttpResponseMessage Get()
        {
            string retVal;
            try
            {
               retVal = ObjectId.GenerateNewId().ToString();
            }

            catch(Exception e)
            {
                WebApiApplication.GlobalLogger.Error(Request+e.ToString());
                HttpError err = new HttpError("Error while generating Id");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }

            return Request.CreateResponse(HttpStatusCode.Created, retVal);
        }  
    }
}
