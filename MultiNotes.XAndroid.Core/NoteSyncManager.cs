using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MultiNotes.Model;
using MultiNotes.XAndroid.Core.Api;

namespace MultiNotes.XAndroid.Core
{
    public class NoteSyncManager
    {
        public void Sync()
        {
            Sync2();
            Sync2();
            Sync2();
        }

        public async void Sync2()
        {
            RemoteNoteRepository remote = new RemoteNoteRepository();
            LocalNoteRepository local = new LocalNoteRepository();


            string bsonId;
            try
            {
                bsonId = await new UniqueIdApi().GetUniqueId();
                
            }
            catch (WebApiClientException)
            {
                bsonId = await new LocalUniqueIdService().GetUniqueId();
            }

            string token = await new AuthTokenApi().GetAuthToken(new AuthenticationRecord()
            {
                Email = new Authorization().UserEmailAddress,
                PasswordHash = new Authorization().UserPasswordHash
            });

            List<Note> remoteList = remote.GetAllNotes(token);
            List<Note> localList = local.GetAllNotes();

            Note note = new Note()
            {
                Id = bsonId,
                Content = "1",
                OwnerId = new Authorization().UserId,
                CreateTimestamp = DateTime.Now,
                LastChangeTimestamp = DateTime.Now
            };

            remote.AddNote(note, token);
            remoteList = remote.GetAllNotes(token);

            note.Content = "2";

            remote.UpdateNote(note, token);
            remoteList = remote.GetAllNotes(token);

            remote.DeleteNote(note, token);
            remoteList = remote.GetAllNotes(token);
        }
    }
}
