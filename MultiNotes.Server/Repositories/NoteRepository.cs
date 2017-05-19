using MongoDB.Driver;
using MultiNotes.Core;
using System.Collections.Generic;

namespace MultiNotes.Server.Repositories
{
    public class NoteRepository : INoteRepository
    {
        protected IMongoCollection<INote> _notesCollection;

        public NoteRepository(IMongoDatabase database, string collectionName="Notes")
        {
            _notesCollection = database.GetCollection<INote>(collectionName);
        }

        public IEnumerable<INote> GetAllNotes()
        {
            return _notesCollection.Find(n => true).ToList();
        }

        public IEnumerable<INote> GetAllNotes(User user)
        {
            return _notesCollection.Find(n => n.OwnerId==user.Id).ToList();
        }

        public INote GetNote(string id)
        {
            return _notesCollection.Find(n => n.Id == id).SingleOrDefault<INote>();
        }

        public INote AddNote(INote item)
        {
            _notesCollection.InsertOne(item);
            return item;
        }

        public void RemoveNote(string id)
        {
            _notesCollection.DeleteOne(n => n.Id == id);
        }

        public void UpdateNote(string id, INote item)
        {
            _notesCollection.FindOneAndReplace(b => b.Id == id, item);
        }

        public bool CheckForNote(string id)
        {
            if (_notesCollection.Count(n => n.Id == id) == 0)
                return false;
            else
                return true;
        }
    }
}