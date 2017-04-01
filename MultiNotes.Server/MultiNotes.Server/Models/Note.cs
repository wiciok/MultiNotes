using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

// todo: zastanowic sie czy nie uzyc jednego modelu dla serwera i klienta


namespace MultiNotes.Server
{
    public class Note
    {
        [BsonId]
        public string Id { get; set; }

        // tutaj byc moze bedzie referencja do wlasciciela notatki
        // na razie niepotrzebne
        // public string OwnerId { get; set; }

        public string Content { get; set; }
        public DateTime CreateTimestamp { get; }
        public DateTime LastChangeTimestamp { get; set; }
    }
}