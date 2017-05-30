using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace MultiNotes.Model
{
    public class Note : INote
    {
        [BsonId, JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "OwnerId")]
        public string OwnerId { get; set; }

        [JsonProperty(PropertyName = "Content")]
        public string Content { get; set; }

        [JsonProperty(PropertyName = "CreateTimestamp")]
        public DateTime CreateTimestamp { get; set; }

        [JsonProperty(PropertyName = "LastChangeTimestamp")]
        public DateTime LastChangeTimestamp { get; set; }

        public override string ToString()
        {
            return "id: " + Id + "OnwerId: " + OwnerId + " Content: " + Content;
        }
    }
}