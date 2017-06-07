using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiNotes.XAndroid.Core
{
    internal sealed class Constants
    {
        public static string AuthenticationRecordFile { get { return GetAuthRecordFile(); } }
        public static string NotesFile { get { return GetNotesFile(); } }
        public static string LocalIdFile { get { return GetLocalIdFile(); } }
        public static string ApiUrlBase { get { return GetApiUrlBase(); } }

        private static string GetAuthRecordFile()
        {
            return System.IO.Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData),
                "MultiNotes", 
                "user.dat"
            );
        }

        private static string GetNotesFile()
        {
            return System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                "MultiNotes",
                "notes.dat"
            );
        }

        private static string GetLocalIdFile()
        {
            return System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "MultiNotes",
                "localid.dat"
            );
        }

        private static string GetApiUrlBase()
        {
            return "http://217.61.4.233:8080/MultiNotes.Server/";
            //return "http://192.168.0.9:63252/";
        }

        private Constants()
        {
        }
    }
}
