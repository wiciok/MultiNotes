using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MultiNotes.Model;
using MultiNotes.Server.Repositories;

namespace MultiNotes.Server
{
    //warto stosowac static czy nie?
    public static class NoteArchiviseService
    {
        private static readonly INoteArchiveRepository NotesArchiveRepo = UnitOfWork.Instance.NotesArchiveRepository;

        public static void ArchiviseNote(Note note, NotesOperations operation)
        {
            NoteArchival noteArch = new NoteArchival(note, operation, ObjectId.GenerateNewId().ToString());
            NotesArchiveRepo.AddNote(noteArch);
        }
    }
}