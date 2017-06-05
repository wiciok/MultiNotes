using MultiNotes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiNotes.XAndroid.Core.Api;

namespace MultiNotes.XAndroid.Core
{
    public class NoteSync
    {
        /// <exception cref="WebApiClientException"></exception>
        public void Sync()
        {
            XNoteMethod noteMethod = new XNoteMethod();

            string token = new AuthTokenApi().GetAuthToken(
                new AuthenticationRecord()
                {
                    Email = new Authorization().UserEmailAddress,
                    PasswordHash = new Authorization().UserPasswordHash
                }
            );

            List<Note> remoteNotes = noteMethod.GetAllRemoteNotes(token);
            List<Note> localNotes = noteMethod.FetchAllLocalNotes();

            foreach (Note localNote in localNotes)
            {
                if (localNote.CreateTimestamp != DateTime.MinValue)
                {
                    // If note is saved only on local device and not on remote server
                    if (localNote.Id.Contains(LocalUniqueIdApi.LocalModifier))
                    {
                        noteMethod.DeleteLocalNote(localNote);
                        localNote.Id = new UniqueIdApi().GetUniqueId();
                        noteMethod.AddLocalNote(localNote);
                        noteMethod.AddRemoteNote(localNote, token);
                    }
                    else
                    {
                        if (ContainsNoteById(localNote.Id, remoteNotes))
                        {
                            if (remoteNotes.Where(g => g.Id == localNote.Id).First()
                                .LastChangeTimestamp < localNote.LastChangeTimestamp)
                            {
                                noteMethod.UpdateRemoteNote(localNote, token);
                            }
                        }
                        else
                        {
                            noteMethod.DeleteLocalNote(localNote);
                        }
                    }
                }
                else
                {
                    noteMethod.DeleteLocalNote(localNote);
                    noteMethod.DeleteRemoteNote(localNote, token);
                }
            }
            
            remoteNotes = noteMethod.GetAllRemoteNotes(token);
            localNotes = noteMethod.FetchAllLocalNotes();
            
            foreach (Note remoteNote in remoteNotes)
            {
                if (!ContainsNoteById(remoteNote.Id, localNotes))
                {
                    noteMethod.AddLocalNote(remoteNote);
                }
                else
                {
                    if (ContainsNoteById(remoteNote.Id, remoteNotes))
                    {
                        if (remoteNotes.Where(g => g.Id == remoteNote.Id).First()
                            .LastChangeTimestamp < remoteNote.LastChangeTimestamp)
                        {
                            noteMethod.UpdateLocalNote(remoteNote);
                        }
                    }
                    else
                    {
                        noteMethod.DeleteRemoteNote(remoteNote, token);
                    }
                }
            }

            /*
            foreach (var remoteNote in remoteNotes)
            {
                foreach (var localNote in localNotes)
                {
                    // Update notes that changed and the newer version is stored on server
                    if (remoteNote.Id == localNote.Id && localNote.LastChangeTimestamp < remoteNote.LastChangeTimestamp)
                    {
                        noteMethod.UpdateLocalNote(remoteNote);
                    }
                    // Update notes that changed and the newer version is stored locally
                    if (remoteNote.Id == localNote.Id && localNote.LastChangeTimestamp > remoteNote.LastChangeTimestamp)
                    {
                        noteMethod.UpdateRemoteNote(localNote, token);
                    }
                    // Update notes that exist only locally 
                    if (!ContainsNoteById(remoteNote.Id, localNotes))
                    {
                        noteMethod.AddRemoteNote(remoteNote, token);
                    }
                    // Update notes that exist only on server
                    if (!ContainsNoteById(localNote.Id, remoteNotes))
                    {
                        noteMethod.AddLocalNote(localNote);
                    }
                }
            }
            */
        }

        private bool ContainsNoteById(string noteId, IEnumerable<Note> notes)
        {
            foreach (Note note in notes)
            {
                if (note.Id == noteId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
