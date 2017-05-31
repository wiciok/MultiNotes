using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Newtonsoft.Json;

using MultiNotes.Model;

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
                List<Note> listNotes = tmpList.Select(JsonConvert.DeserializeObject<Note>).ToList();
                return tmpList.Select(JsonConvert.DeserializeObject<Note>).Where(a => a.OwnerId == "-1").ToList();
            }

            return new List<Note>();
        }


        private void RestoreStaticNotesInFileField()
        {
            NotesInFile = FetchAllNotes();
        }


        public void AddNote(Note note)
        {
            PrepareNoteToAdd(ref note);
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


        private void PrepareNoteToAdd(ref Note note)
        {
            note.Id = new LocalUniqueIdService().GetUniqueId();
            note.OwnerId = "-1";
            note.CreateTimestamp = DateTime.Now;
            note.LastChangeTimestamp = DateTime.Now;
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