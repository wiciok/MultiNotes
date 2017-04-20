using MongoDB.Bson;
using MongoDB.Driver;
using MultiNotes.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultiNotes.Server.Repositories
{
    //todo: poprzerabiac te metody jakby find nei zwracalo niczego, bo bedzie rzucony wyjatek!!

    public class NoteRepository : INoteRepository
    {
        private IMongoCollection<Note> _notesCollection;

        public NoteRepository(IMongoDatabase database) //dependency injection
        {
            _notesCollection = database.GetCollection<Note>("Notes");
        }

        // TODO: przemyslec async / await.
        public IEnumerable<Note> GetAllNotes()
        {
            return _notesCollection.Find(n => true).ToList();
        }

        public IEnumerable<Note> GetAllNotes(User user)
        {
            return _notesCollection.Find(n => n.OwnerId==user.Id).ToList();
        }

        public Note GetNote(string id)
        {
            return _notesCollection.Find(n => n.Id == id).Single<Note>();
        }

        public Note GetNote(string id, User user)
        {
            return _notesCollection.Find(n => n.Id == id).Single<Note>();
        }

        public Note AddNote(Note item)
        {
            _notesCollection.InsertOne(item);
            return item;
        }

        public void RemoveNote(string id)
        {
            _notesCollection.DeleteOne(n => n.Id == id);
        }

        public void UpdateNote(string id, Note item)
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