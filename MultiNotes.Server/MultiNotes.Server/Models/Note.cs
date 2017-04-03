using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

// TODO: zastanowic sie czy nie uzyc jednego modelu dla serwera i klienta.

namespace MultiNotes.Server.Models
{
    public class Note
    {
        [BsonId]
        public string Id { get; set; }

        // Tutaj byc moze bedzie referencja do wlasciciela notatki.

        // Na razie niepotrzebne.
        // public string OwnerId { get; set; }

        public string Content { get; set; }
        public DateTime CreateTimestamp { get; }
        public DateTime LastChangeTimestamp { get; set; }
    }
}