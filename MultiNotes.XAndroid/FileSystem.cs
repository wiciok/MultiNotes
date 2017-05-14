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

using Java.IO;

using MultiNotes.Core;
using Org.Json;

namespace MultiNotes.XAndroid
{
    public class NotesFileSystem : IFileSystem
    {
        private static IFileSystem instance = null;
        private static object syncRoot = new object();
        public static IFileSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new NotesFileSystem();
                        }
                    }
                }
                return instance;
            }
        }

        private NotesFileSystem()
        {
            filename = "MultiNotes.dat";
        }


        private File filesDir;
        private string filename;

        public List<Note> GetSavedNotes()
        {
            File file = new File(filesDir, filename);
            try
            {
                FileOutputStream outputStream = new FileOutputStream(file);
            }
            catch (Exception)
            {
                //
            }
            throw new NotImplementedException();
        }

        public void SaveNotes(List<Note> notes)
        {
            File file = new File(filesDir, filename);
            try
            {
                FileOutputStream outputStream = new FileOutputStream(file);
                JSONObject jsonObject = new JSONObject();
                foreach (Note note in notes)
                {
                }
            }
            catch (Exception)
            {
                //
            }
        }

        public File FilesDir
        {
            get { return filesDir; }
            set { filesDir = value; }
        }
    }
}