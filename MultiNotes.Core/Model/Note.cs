using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;


namespace MultiNotes.Core
{
    public class Note
    {
        [BsonId]
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public string Content { get; set; }
        public DateTime CreateTimestamp { get; set; }
        public DateTime LastChangeTimestamp { get; set; }

        public override string ToString()
        {
            return "id: "+Id+"OnwerId: "+OwnerId+" Content: "+Content;
        }

        //todo: do rozwiazania kwestia referencji do wlasciciela notatki - na razie jest ownerid
        /*public User Owner
        {
            get
            {
                return 
            }
        }*/
    }
}