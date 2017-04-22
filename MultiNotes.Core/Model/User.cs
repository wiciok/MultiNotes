using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

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
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime RegistrationTimestamp { get; }
    }
}