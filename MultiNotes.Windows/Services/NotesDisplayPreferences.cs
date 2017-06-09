using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using MultiNotes.Model;
using Newtonsoft.Json;

namespace MultiNotes.Windows.Services
{
    public class NoteWindowPreferences
    {
        public double WindowPositionX;
        public double WindowPositionY;
        public double WindowWidth;
        public double WindowHeight;
        public Brush WindowColor;
        public bool IsDisplayed;
    }

    public static class NotesDisplayPreferences
    {
        private static List<Tuple<string, NoteWindowPreferences>> _notePreferencesList;
        private static List<Tuple<string, NoteWindowPreferences>> NotePreferencesList
        {
            get => _notePreferencesList ??
                (_notePreferencesList = new List<Tuple<string, NoteWindowPreferences>>());
            set => _notePreferencesList = value;
        }

        private static readonly string Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MultiNotes", "notePreferences.cfg");

        static NotesDisplayPreferences()
        {
            _notePreferencesList = new List<Tuple<string, NoteWindowPreferences>>();
            ReadFromDisc();
        }

        public static void Add(string noteId, NoteWindowPreferences noteWindowPreferences)
        {
            if (_notePreferencesList.Count(x => x.Item1 == noteId) != 0)
            {
                var tmp = _notePreferencesList.Find(x => x.Item1 == noteId);
                _notePreferencesList.Remove(tmp);
            }

            _notePreferencesList.Add(new Tuple<string, NoteWindowPreferences>(noteId, noteWindowPreferences));

        }

        public static NoteWindowPreferences Get(string noteId)
        {
            var tuple = NotePreferencesList.FirstOrDefault(x => x.Item1 == noteId);
            return tuple?.Item2;
        }

        public static void SaveToDisc()
        {
            var json = JsonConvert.SerializeObject(_notePreferencesList);

            if (!File.Exists(Path))
                File.Create(Path);

            try
            {
                File.WriteAllText(Path,json);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error writing into preferences file!");
                //throw;
            }
        }

        private static void ReadFromDisc()
        {
            try
            {
                if (File.Exists(Path))
                {
                    var json = File.ReadAllText(Path);
                    _notePreferencesList = JsonConvert.DeserializeObject<List<Tuple<string, NoteWindowPreferences>>>(json);

                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error reading preferences file!");
                //throw;
            }
        }
    }
}
