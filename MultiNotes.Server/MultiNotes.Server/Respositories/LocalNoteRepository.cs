using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultiNotes.Server.Respositories
{
    /**
     * Nie mam na razie bazy danych MongoDB na swoim komputerze, więc
     * zrobiłem sobie taką klasę, do obsługi repozytorium notatek.
     * 
     * ~ Damian Malczewski, 2017-04-01
     */
    public class LocalNoteRepository : INoteRepository
    {
        private ICollection<Note> _notes;

        public LocalNoteRepository()
        {
            _notes = new List<Note>();
            Initialize();
        }


        public Note AddNote(Note item)
        {
            _notes.Add(item);
            return item;
        }

        public IEnumerable<Note> GetAllNotes()
        {
            return _notes.AsEnumerable();
        }

        public Note GetNote(string id)
        {
            return _notes.Where(n => n.Id == id).First();
        }

        public bool RemoveNote(string id)
        {
            int changeCounter = 0;
            foreach (Note note in _notes.Where(n => n.Id == id))
            {
                if (!_notes.Remove(note))
                {
                    return false;
                }
                changeCounter++;
            }
            return changeCounter != 0;
        }

        public bool UpdateNote(string id, Note item)
        {
            int changeCounter = 0;
            foreach (Note note in _notes.Where(n => n.Id == id))
            {
                note.Content = item.Content;
                note.LastChangeTimestamp = item.LastChangeTimestamp;
                changeCounter++;
            }
            return changeCounter != 0;
        }


        private void Initialize()
        {
            _notes.Add(new Note()
            {
                Id = "1",
                Content = "Kup mleko",
                LastChangeTimestamp = new DateTime(2017, 04, 01, 11, 12, 22)
            });
            _notes.Add(new Note()
            {
                Id = "2",
                Content = "Idź do lekarza",
                LastChangeTimestamp = new DateTime(2017, 05, 03, 13, 11, 33)
            });
            _notes.Add(new Note()
            {
                Id = "3",
                Content = "Zapłać rachunki",
                LastChangeTimestamp = new DateTime(2017, 04, 21, 12, 14, 18)
            });
            _notes.Add(new Note()
            {
                Id = "4",
                Content = "Rozmowa o pracę",
                LastChangeTimestamp = new DateTime(2017, 04, 30, 08, 08, 08)
            });
            _notes.Add(new Note()
            {
                Id = "5",
                Content = "Adopcja dziecka",
                LastChangeTimestamp = new DateTime(2017, 07, 28, 15, 11, 34)
            });
        }
    }
}