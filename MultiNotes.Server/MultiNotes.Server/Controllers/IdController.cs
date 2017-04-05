using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using MongoDB.Bson;


namespace MultiNotes.Server
{
    //class returning bsonId, used for creating new notes or users in client apps
    public class IdController : ApiController
    {
        public string Get()
        {
            return ObjectId.GenerateNewId().ToString();
        }  
    }
}
