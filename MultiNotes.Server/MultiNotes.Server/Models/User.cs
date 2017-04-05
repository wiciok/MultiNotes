using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MultiNotes.Server.Models
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