using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace MultiNotes.Model
{
    public class User
    {

        [BsonId, JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "EmailAddress")]
        public string EmailAddress { get; set; } //also login

        [JsonProperty(PropertyName = "PasswordHash")]
        public string PasswordHash { get; set; }

        [JsonProperty(PropertyName = "RegistrationTimestamp")]
        public DateTime RegistrationTimestamp { get; set; }
    }
}