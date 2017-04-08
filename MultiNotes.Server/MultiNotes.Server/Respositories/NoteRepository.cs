﻿using MongoDB.Bson;
using MongoDB.Driver;
using MultiNotes.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultiNotes.Server.Respositories
{
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
        public Note AddNote(Note item)
        {
            // Byc moze do jakichs zmian.
            //item.LastChangeTimestamp = DateTime.Now;
            //item.Id = ObjectId.GenerateNewId().ToString();

            _notesCollection.InsertOne(item);
            return item;
        }

        public bool RemoveNote(string id)
        {
            var result = _notesCollection.DeleteOne(n => n.Id == id);
            return result.DeletedCount == 1;
        }

        public bool UpdateNote(string id, Note item)
        {
            throw new NotImplementedException();
            return false;
        }
    }
}