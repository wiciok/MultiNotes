using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace MultiNotes.Core
{
    public class User
    {
        [BsonId]
        public string Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}