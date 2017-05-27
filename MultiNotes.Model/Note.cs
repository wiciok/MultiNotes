using System;
using MongoDB.Bson.Serialization.Attributes;

namespace MultiNotes.Model
{
    public class Note: INote
    {
        [BsonId]
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public string Content { get; set; }      
        public DateTime CreateTimestamp { get; set; }
        public DateTime LastChangeTimestamp { get; set; }

        public override string ToString()
        {
            return "id: " + Id + "OnwerId: " + OwnerId + " Content: " + Content;
        }
    }
}