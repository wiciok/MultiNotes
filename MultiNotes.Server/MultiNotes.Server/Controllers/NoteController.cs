using MultiNotes.Server.Models;
using MultiNotes.Server.Respositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MultiNotes.Server
{
    public class NoteController : ApiController
    {
        // Sposob uzycia tego repo moze ulec zmianie.
        private static readonly INoteRepository repo = new NoteRepository();

        // IQueryable zamiast IEnumerable - zwiększona wydajność.
        public IQueryable<Note> Get()
        {
            return repo.GetAllNotes().AsQueryable();
        }

        // GET api/note/5
        public Note Get(int id)
        {
            // TODO: przemyslec czy tutaj string czy int.

            Note note = repo.GetNote(id.ToString());
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
        public void Delete(int id)
        {
            // TODO: przemyslec czy tutaj string czy int.

            if (!repo.RemoveNote(id.ToString()))
                throw new HttpResponseException(HttpStatusCode.NotFound);
        }
    }
}
