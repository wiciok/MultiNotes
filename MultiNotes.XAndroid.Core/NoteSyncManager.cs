using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MultiNotes.Model;

namespace MultiNotes.XAndroid.Core
{
    public class NoteSyncManager
    {
        public void Sync(INoteRepository remote, INoteRepository local)
        {
            List<Note> remoteList = remote.GetAllNotes();
            List<Note> localList = local.GetAllNotes();

            remote.AddNote(new Note()
            {
                Content = "1",
                CreateTimestamp = DateTime.Now,
                LastChangeTimestamp = DateTime.Now
            });
            remoteList = remote.GetAllNotes();
        }
    }
}
