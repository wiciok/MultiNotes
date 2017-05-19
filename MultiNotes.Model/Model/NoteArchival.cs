using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace MultiNotes.Core
{
    public enum NotesOperations
    {
        Update = 0, Delete = 1
    }

    public class NoteArchival : INote
    {
        public NoteArchival(Note originalNote, NotesOperations operation, string id)
        {
            this.Id = id;
            this.OriginalNoteId = originalNote.Id;
            this.OwnerId = originalNote.OwnerId;
            this.Content = originalNote.Content;
            this.CreateTimestamp = originalNote.CreateTimestamp;
            this.LastChangeTimestamp = originalNote.LastChangeTimestamp;

            this.MoveToArchiveTimeStamp = DateTime.Now;
            this._noteOperation = operation;
        }

        [BsonId]
        public string Id { get; private set; }
        public string OriginalNoteId { get; }
        public string OwnerId { get; }
        public string Content { get; }
        public DateTime CreateTimestamp { get; }
        public DateTime LastChangeTimestamp { get; }
        public DateTime MoveToArchiveTimeStamp { get; }

        private readonly NotesOperations _noteOperation;
        public string NoteOperation
        {
            get
            {
                switch (_noteOperation)
                {
                    case NotesOperations.Update:
                        return "Update";
                        break;
                    case NotesOperations.Delete:
                        return "Delete";
                        break;
                    default:
                        return "";
                }
            }
        }

        public override string ToString()
        {
            return "id: " + Id + "OnwerId: " + OwnerId + " Content: " + Content + "Moved to archive at: " + MoveToArchiveTimeStamp.ToString();
        }
    }
}
