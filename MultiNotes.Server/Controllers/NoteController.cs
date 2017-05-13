using MultiNotes.Core;
using MultiNotes.Server.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MultiNotes.Server.Models;
using System.Web.Http.Description;

namespace MultiNotes.Server
{
    [RoutePrefix("api/token")]
    public class NoteController : ApiController
    {
        private static readonly INoteRepository notesRepo = UnitOfWork.Instance.NotesRepository;
        private static readonly IUserRepository usersRepo = UnitOfWork.Instance.UsersRepository;
        private AuthorizationService authService = new AuthorizationService(usersRepo);

        // IQueryable zamiast IEnumerable - zwiększona wydajność.
        //GET /api/note/{token}
        [Route("{token}")]
        [ResponseType(typeof(IQueryable<Note>))]
        public HttpResponseMessage Get([FromUri]string token)
        {
            try
            {
                if (authService.CheckAuthorization(token) == true)
                {
                    return Request.CreateResponse<IQueryable<Note>>(HttpStatusCode.OK, notesRepo.GetAllNotes(authService.currentUser).AsQueryable());
                }
                else
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            catch
            {
                HttpError err = new HttpError("Error while getting all user's notes");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }           
        }

        //GET /api/token/{token}/{id}
        [Route("{token}/{id}")]
        [ResponseType(typeof(Note))]
        public HttpResponseMessage Get([FromUri] string token, [FromUri]string id)
        {
            try
            {
                if (authService.CheckAuthorization(token) == false)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);

                if (notesRepo.CheckForNote(id) == false)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                Note note = notesRepo.GetNote(id);

                if (note.OwnerId == authService.currentUser.Id)
                    return Request.CreateResponse<Note>(HttpStatusCode.OK,note);
                else
                    return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            catch
            {
                HttpError err = new HttpError("Error while getting specific note");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }           
        }

        [Route("{note}/{token}")]   
        // POST api/note
        public HttpResponseMessage Post([FromUri]string token, [FromBody]Note value)
        {
            try
            {
                if (authService.CheckAuthorization(token) == false)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);

                if (notesRepo.CheckForNote(value.Id) == true)
                {
                    if (authService.currentUser.Id == value.OwnerId)
                        notesRepo.UpdateNote(value.Id, value);
                    else
                        return Request.CreateResponse(HttpStatusCode.Forbidden);
                }
                else
                    notesRepo.AddNote(value);

                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch
            {
                HttpError err = new HttpError("Error while posting note");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }
        }


        [Route("{token}/{id}")]
        // DELETE api/note/5
        public HttpResponseMessage Delete([FromUri]string token, [FromUri]string id)
        {
            try
            {
                if (authService.CheckAuthorization(token) == false)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);

                if (notesRepo.CheckForNote(id) == true)
                {
                    Note note = notesRepo.GetNote(id);
                    if (authService.currentUser.Id == note.OwnerId)
                    {
                        notesRepo.RemoveNote(id);
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }      
                    else
                        return Request.CreateResponse(HttpStatusCode.Forbidden);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch
            {
                HttpError err = new HttpError("Error while deleting note");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }
        }
    }
}
