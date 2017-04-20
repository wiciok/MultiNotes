using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using MongoDB.Bson;
using System.Net.Http;
using System.Web.Http.Description;

namespace MultiNotes.Server
{
    //class returning bsonId, used for creating new notes or users in client apps
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

            catch
            {
                HttpError err = new HttpError("Error while generating Id");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }

            return Request.CreateResponse<string>(HttpStatusCode.Created, retVal);
        }  
    }
}
