using MongoDB.Driver;
using System.Collections.Generic;
using MultiNotes.Model;

namespace MultiNotes.Server.Repositories
{
    public class NoteRepository : INoteRepository
    {
        protected IMongoCollection<INote> NotesCollection;

        public NoteRepository(IMongoDatabase database, string collectionName="Notes")
        {
            NotesCollection = database.GetCollection<INote>(collectionName);
        }

        private bool CheckForAnyNote()
        {
            return NotesCollection.Count(n => true) != 0;
        }

        public bool CheckForAnyUserNote(User user)
        {
            if (CheckForAnyNote())
                return NotesCollection.Count(n => n.OwnerId.Equals(user.Id)) != 0;
            return false;
        }

        public IEnumerable<INote> GetAllNotes()
        {
            return NotesCollection.Find(n => true).ToList();
        }

        public IEnumerable<INote> GetAllNotes(User user)
        {
            return NotesCollection.Find(n => n.OwnerId==user.Id).ToList();
        }

        public INote GetNote(string id)
        {
            return NotesCollection.Find(n => n.Id == id).SingleOrDefault<INote>();
        }

        public INote AddNote(INote item)
        {
            NotesCollection.InsertOne(item);
            return item;
        }

        public void RemoveNote(string id)
        {
            NotesCollection.DeleteOne(n => n.Id == id);
        }

        public void UpdateNote(string id, INote item)
        {
            NotesCollection.FindOneAndReplace(b => b.Id == id, item);
        }

        public bool CheckForNote(string id)
        {
            if(CheckForAnyNote())
                return NotesCollection.Count(n => n.Id == id) != 0;
            return false;
        }
    }
}