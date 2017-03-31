using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MultiNotes.Server
{
    public class MongoDBConfig
    {   //klasa testowa, nie wiadomo na razie czy zostanie
        //todo: po ustawieniu uzytkownikow w bazie danych, zmienic tutaj connectionString;
        private static string ConnectionString = "mongodb://localhost:27017";

        public static void InitDatabaseConnection()
        {
            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase("MultiNotes");

            var collection = database.GetCollection<BsonDocument>("test");

            var document = new BsonDocument
            {
                { "name", "MongoDB" },
                { "type", "Database" },
                { "count", 1 }
            };

            collection.InsertOneAsync(document);
        }

        public static void InsertExampleNotes()
        {
            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase("MultiNotes");

            var collection = database.GetCollection<Note>("Notes");
            collection.InsertOneAsync(new Note { Id = "1", Content = "test1", LastChangeTimestamp=DateTime.Now });
            collection.InsertOneAsync(new Note { Id = "2", Content = "test2", LastChangeTimestamp = DateTime.Now });
        }
}


}