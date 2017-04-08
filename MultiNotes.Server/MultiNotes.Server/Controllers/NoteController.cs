using MultiNotes.Server.Respositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MultiNotes.Core;

namespace MultiNotes.Server
{
    //klasa zwraca notatki na podstawie ich id
    //brak jakiejkolwiek autentykacji - docelowo klasa zostanie usunięta, a dostep bedzie sie odbywal przez /api/token/

    public class NoteController : ApiController
    {
        private static readonly INoteRepository repo = UnitOfWork.Instance.NotesRepository;

        // IQueryable zamiast IEnumerable - zwiększona wydajność.
        public IQueryable<Note> Get()
        {
            return repo.GetAllNotes().AsQueryable();
        }

        // GET api/note/5
        public Note Get(string id)
        {
            Note note = repo.GetNote(id);
            if (note == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return note; 
        }

        // POST api/note
        // public void Post([FromBody]Note value)
        public Note Post(Note value)
        {
            return repo.AddNote(value);
        }

        // PUT api/note/5
        public void Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException("http put = update in repository");
        }

        // DELETE api/note/5
        public void Delete(string id)
        {
            // TODO: przemyslec czy tutaj string czy int.

            if (!repo.RemoveNote(id))
                throw new HttpResponseException(HttpStatusCode.NotFound);
        }
    }
}
