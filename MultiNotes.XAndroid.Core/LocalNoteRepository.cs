using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using MultiNotes.Model;
using Newtonsoft.Json;

namespace MultiNotes.XAndroid.Core
{
    public class LocalNoteRepository : INoteRepository
    {
        private static List<Note> NotesInFile = FetchAllNotes();

        public List<Note> GetAllNotes()
        {
            return new List<Note>(NotesInFile);
        }

        private static List<Note> FetchAllNotes()
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
                return listNotes.Where(a => a.OwnerId == "-1").ToList();
            }

            return new List<Note>();
        }

        private static void RestoreStaticNotesInFileField()
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
        }


        public void UpdateNote(Note note)
        {
            List<Note> notesList = GetAllNotes();
            Note noteInFile = notesList.Where(g => g.Id == note.Id).FirstOrDefault();
            noteInFile.Content = note.Content;
            noteInFile.LastChangeTimestamp = DateTime.Now;
            ResaveAllNotes(notesList);
        }


        public void DeleteNote(Note note)
        {
            List<Note> notesList = GetAllNotes();
            notesList.RemoveAll(g => g.Id == note.Id);
            ResaveAllNotes(notesList);
        }

    }
}