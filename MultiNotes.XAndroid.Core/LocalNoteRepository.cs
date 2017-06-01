using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

using MultiNotes.Model;
using MultiNotes.XAndroid.Core.Api;

namespace MultiNotes.XAndroid.Core
{
    public class LocalNoteRepository : INoteRepository
    {
        private List<Note> NotesInFile;
        

        public LocalNoteRepository()
        {
             NotesInFile = FetchAllNotes();
        }


        public bool Success { get; private set; }


        public List<Note> GetAllNotes()
        {
            Success = true;
            return new List<Note>(NotesInFile);
        }


        private List<Note> FetchAllNotes()
        {
            if (System.IO.File.Exists(Constants.NotesFile))
            {
                string json = System.IO.File.ReadAllText(Constants.NotesFile);
                List<string> tmpList = json.Split('}').ToList();
                for (int i = 0; i < tmpList.Count; ++i)
                {
                    tmpList[i] += "}";
                }
                tmpList.RemoveAt(tmpList.Count - 1);
                return tmpList.Select(JsonConvert.DeserializeObject<Note>)
                    .Where(a => a.OwnerId == new Authorization().User.Id).ToList();
            }
            return new List<Note>();
        }


        private void RestoreStaticNotesInFileField()
        {
            NotesInFile = FetchAllNotes();
        }


        public async void AddNote(Note note)
        {
            note.Id = await new UniqueIdApi().GetUniqueId();
            note.CreateTimestamp = DateTime.Now;
            note.LastChangeTimestamp = DateTime.Now;
            note.OwnerId = new Authorization().UserId;

            if (!System.IO.File.Exists(Constants.NotesFile))
            {
                string json = JsonConvert.SerializeObject(note);
                System.IO.File.WriteAllText(Constants.NotesFile, json);
            }
            else
            {
                List<Note> notesList = GetAllNotes();
                notesList.Add(note);
                ResaveAllNotes(notesList);
            }
            Success = true;
        }


        private void ResaveAllNotes(List<Note> notesList)
        {
            System.IO.File.WriteAllText(Constants.NotesFile, "");
            foreach (Note x in notesList)
            {
                string jsonNote = JsonConvert.SerializeObject(x);
                System.IO.File.AppendAllText(Constants.NotesFile, jsonNote);
            }
            RestoreStaticNotesInFileField();
            Success = true;
        }


        public void UpdateNote(Note note)
        {
            note.CreateTimestamp = DateTime.Now;
            note.LastChangeTimestamp = DateTime.Now;

            List<Note> notesList = GetAllNotes();
            Note noteInFile = notesList.Where(g => g.Id == note.Id).FirstOrDefault();
            noteInFile.Content = note.Content;
            noteInFile.LastChangeTimestamp = DateTime.Now;
            ResaveAllNotes(notesList);
            Success = true;
        }


        public void DeleteNote(Note note)
        {
            List<Note> notesList = GetAllNotes();
            notesList.RemoveAll(g => g.Id == note.Id);
            ResaveAllNotes(notesList);
            Success = true;
        }

    }
}