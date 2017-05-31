using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiNotes.XAndroid.Core
{
    public class Setup
    {
        public static void Run()
        {
            CreateDirectoryForFile(Constants.AuthenticationRecordFile);
            CreateDirectoryForFile(Constants.LocalIdFile);
            CreateDirectoryForFile(Constants.NotesFile);
        }

        private static void CreateDirectoryForFile(string pathToFile)
        {
            string directoryName = System.IO.Path.GetDirectoryName(pathToFile);
            if (!System.IO.Directory.Exists(directoryName))
            {
                System.IO.Directory.CreateDirectory(directoryName);
            }
        }
    }
}