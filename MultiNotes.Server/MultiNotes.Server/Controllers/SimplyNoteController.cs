using MultiNotes.Server.Models;
using MultiNotes.Server.Respositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MultiNotes.Server.Controllers
{
    public class SimplyNoteController : ApiController
    {
        private static readonly INoteRepository repository = new LocalNoteRepository();

        // IQueryable zamiast IEnumerable - zwiększona wydajność.
        public IQueryable<Note> Get()
        {
            return repository.GetAllNotes().AsQueryable();
        }

        // GET api/note/5
        public Note Get(int id)
        {
            // TODO: przemyslec czy tutaj string czy int.

            Note note = repository.GetNote(id.ToString());
            if (note == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return note;
        }

        // POST api/note
        // public void Post([FromBody]Note value)
        public Note Post(Note value)
        {
            return repository.AddNote(value);
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

            if (!repository.RemoveNote(id.ToString()))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}
