using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using MultiNotes.Model;

namespace MultiNotes.XAndroid.Core
{
    public class XNoteMethod
    {
        public List<Note> GetAllNotesFromFile(string userId)
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
                return listNotes.Where(a => a.OwnerId == userId).ToList();
            }

            return new List<Note>();
        }


        public void AddNoteToFile(Note note)
        {
            string json = JsonConvert.SerializeObject(note);
            if (!System.IO.File.Exists(Constants.NotesFile))
            {
                System.IO.File.WriteAllText(Constants.NotesFile, json);
            }
            else
            {
                System.IO.File.AppendAllText(Constants.NotesFile, json);
            }
        }


        public void UpdateNoteFromFile(string id, string userId, Note newNote)
        {
            if (!System.IO.File.Exists(Constants.NotesFile)) return;
            string json = System.IO.File.ReadAllText(Constants.NotesFile);
            List<string> tmpList = json.Split('}').ToList();
            for (int i = 0; i < tmpList.Count; ++i)
            {
                tmpList[i] += "}";
            }
            tmpList.RemoveAt(tmpList.Count - 1);
            List<Note> listNotes = tmpList.Select(JsonConvert.DeserializeObject<Note>).ToList();
            Note toDelete = listNotes.FirstOrDefault(a => a.Id == id && a.OwnerId == userId);
            if (toDelete == null)
            {
                return;
            }

            listNotes.Remove(toDelete);
            toDelete.Content = newNote.Content;
            toDelete.LastChangeTimestamp = newNote.LastChangeTimestamp;
            listNotes.Add(toDelete);

            System.IO.File.WriteAllText(Constants.NotesFile, "");
            foreach (Note x in listNotes)
            {
                string jsonNote = JsonConvert.SerializeObject(x);
                System.IO.File.AppendAllText(Constants.NotesFile, jsonNote);
            }
        }


        public void DeleteNoteFromFile(string id, string userId)
        {
            if (!System.IO.File.Exists(Constants.NotesFile))
            {
                return;
            }

            string json = System.IO.File.ReadAllText(Constants.NotesFile);
            List<string> tmpList = json.Split('}').ToList();
            for (int i = 0; i < tmpList.Count; ++i)
            {
                tmpList[i] += "}";
            }
            tmpList.RemoveAt(tmpList.Count - 1);
            List<Note> listNotes = tmpList.Select(JsonConvert.DeserializeObject<Note>).ToList();
            Note toDelete = listNotes.FirstOrDefault(a => a.Id == id && a.OwnerId == userId);

            if (toDelete == null)
            {
                return;
            }

            listNotes.Remove(toDelete);
            System.IO.File.WriteAllText(Constants.NotesFile, "");
            foreach (Note x in listNotes)
            {
                string jsonNote = JsonConvert.SerializeObject(x);
                System.IO.File.AppendAllText(Constants.NotesFile, jsonNote);
            }
        }
    }
}