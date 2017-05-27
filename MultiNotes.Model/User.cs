using System;
using MongoDB.Bson.Serialization.Attributes;

namespace MultiNotes.Model
{
    public class User
    {
        public User(string id, string passwordHash, string email)
        {
            Id = id;
            PasswordHash = passwordHash;
            EmailAddress = email;
            RegistrationTimestamp = DateTime.Now;
        }

        [BsonId]
        public string Id { get; }
        public string EmailAddress { get; set; } //also login
        public string PasswordHash { get; set; }
        public DateTime RegistrationTimestamp { get; }
    }
}