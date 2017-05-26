using System;
using MongoDB.Bson.Serialization.Attributes;

namespace MultiNotes.Model
{
    public class Note: INote
    {
        public Note(string id, string ownerId, string content="")
        {
            Id = id;
            OwnerId = ownerId;
            Content = content;
            CreateTimestamp = DateTime.Now;
            LastChangeTimestamp = DateTime.Now;
        }

        private void UpdateChangeTimestamp()
        {
            LastChangeTimestamp = DateTime.Now;
        }

        [BsonId]
        public string Id { get; }
        public string OwnerId { get; }

        private string _content;
        public string Content
        {
            get => _content;
            set
            {
                UpdateChangeTimestamp();
                _content = value;
            }            
        }

        public DateTime CreateTimestamp { get; }
        public DateTime LastChangeTimestamp { get; private set; }

        public override string ToString()
        {
            return "id: " + Id + "OnwerId: " + OwnerId + " Content: " + Content;
        }
    }
}