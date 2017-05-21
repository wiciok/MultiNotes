using MongoDB.Driver;
using System.Collections.Generic;
using MultiNotes.Model;

namespace MultiNotes.Server.Repositories
{
    public interface INoteArchiveRepository
    {
        IEnumerable<INote> GetAllNotes(User user);
        IEnumerable<INote> GetAllVersionsOfNote(string originalNoteId);
        bool CheckForAnyVersionsOfNote(string originalNoteId);
        INote AddNote(NoteArchival item);
    }

    public class NoteArchiveRepository: INoteArchiveRepository
    {
        protected IMongoCollection<NoteArchival> _notesArchiveCollection;

        public NoteArchiveRepository(IMongoDatabase database, string collectionName = "NotesArchive")
        {
            _notesArchiveCollection = database.GetCollection<NoteArchival>(collectionName);
        }

        public IEnumerable<INote> GetAllNotes(User user)
        {
            return _notesArchiveCollection.Find(n => n.OwnerId == user.Id).ToList();
        }

        public IEnumerable<INote> GetAllVersionsOfNote(string originalNoteId)
        {
            return _notesArchiveCollection.Find(n => n.OriginalNoteId == originalNoteId).ToList();
        }

        public INote AddNote(NoteArchival item)
        {
            _notesArchiveCollection.InsertOne(item);
            return item;
        }


        public bool CheckForAnyVersionsOfNote(string originalNoteId)
        {
            if (_notesArchiveCollection.Count(n => n.OriginalNoteId == originalNoteId) == 0)
                return false;
            else
                return true;
        }
    }
}