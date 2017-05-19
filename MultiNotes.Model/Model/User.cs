using System;
using MongoDB.Bson.Serialization.Attributes;

namespace MultiNotes.Core
{
    public class User
    {
        public User()
        {
            this.RegistrationTimestamp = DateTime.Now;
        }

        [BsonId]
        public string Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime RegistrationTimestamp { get; }
    }
}