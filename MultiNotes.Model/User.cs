using System;
using MongoDB.Bson.Serialization.Attributes;

namespace MultiNotes.Model
{
    public class User
    {
        [BsonId]
        public string Id { get; set; }
        public string EmailAddress { get; set; } //also login
        public string PasswordHash { get; set; }
        public DateTime RegistrationTimestamp { get; set; }
    }
}