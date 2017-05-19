using System;
using MongoDB.Bson.Serialization.Attributes;

namespace MultiNotes.Model
{
    public class User
    {
        public User(string id, string login, string passwordHash, string email)
        {
            this.Id = id;
            this.Login = login;
            this.PasswordHash = passwordHash;
            this.EmailAddress = email;
            this.RegistrationTimestamp = DateTime.Now;
        }

        [BsonId]
        public string Id { get; private set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string EmailAddress { get; set; }
        public DateTime RegistrationTimestamp { get; }
    }
}