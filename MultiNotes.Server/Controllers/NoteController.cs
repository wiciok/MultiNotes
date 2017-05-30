using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MultiNotes.Model;
using MultiNotes.Server.Repositories;
using MultiNotes.Server.Services;

namespace MultiNotes.Server.Controllers
{
    [LogWebApiRequest]
    [RoutePrefix("api/note")]
    public class NoteController : ApiController
    {
        private static readonly INoteRepository NotesRepo = UnitOfWork.Instance.NotesRepository;
        private static readonly IUserRepository UsersRepo = UnitOfWork.Instance.UsersRepository;
        private readonly AuthorizationService _authService = new AuthorizationService(UsersRepo);

        // IQueryable zamiast IEnumerable - zwiększona wydajność.
        //GET /api/note/{token}
        [Route("{token}")]
        [ResponseType(typeof(IQueryable<Note>))]
        [HttpGet]
        public HttpResponseMessage Get([FromUri]string token)
        {
            try
            {
                if (_authService.CheckAuthorization(token) == false)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                if (NotesRepo.CheckForAnyNote(_authService.CurrentUser) == false)
                    return Request.CreateResponse(HttpStatusCode.NoContent);
                return Request.CreateResponse(HttpStatusCode.OK,
                    (IQueryable<Note>) NotesRepo.GetAllNotes(_authService.CurrentUser).AsQueryable());
            }
            catch(Exception e)
            {
                WebApiApplication.GlobalLogger.Error(Request + e.ToString());
                var err = new HttpError("Error while getting all user's notes");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }           
        }

        //GET /api/token/{token}/{id}
        [Route("{token}/{id}")]
        [ResponseType(typeof(Note))]
        [HttpGet]
        public HttpResponseMessage Get([FromUri] string token, [FromUri]string id)
        {
            try
            {
                if (_authService.CheckAuthorization(token) == false)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);

                if (NotesRepo.CheckForNote(id) == false)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                var note = (Note)NotesRepo.GetNote(id);

                return note.OwnerId == _authService.CurrentUser.Id
                    ? Request.CreateResponse(HttpStatusCode.OK,note)
                    : Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            catch(Exception e)
            {
                WebApiApplication.GlobalLogger.Error(Request + e.ToString());
                var err = new HttpError("Error while getting specific note");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }           
        }

        [Route("{token}")]
        [ResponseType(typeof(Note))]
        [HttpPost]
        // POST api/note
        public HttpResponseMessage Post([FromUri]string token, [FromBody]Note value)
        {
            try
            {
                if (_authService.CheckAuthorization(token) == false)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);

                if (NotesRepo.CheckForNote(value.Id) == true)
                {
                    if (_authService.CurrentUser.Id == value.OwnerId)
                    {
                        NotesRepo.UpdateNote(value.Id, value);
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                        
                    else
                        return Request.CreateResponse(HttpStatusCode.Forbidden);
                }
                else
                    NotesRepo.AddNote(value);

                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch(Exception e)
            {
                WebApiApplication.GlobalLogger.Error(Request + e.ToString());
                HttpError err = new HttpError("Error while posting note");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }
        }


        [Route("{token}/{id}")]
        [HttpDelete]
        // DELETE api/note/5
        public HttpResponseMessage Delete([FromUri]string token, [FromUri]string id)
        {
            try
            {
                if (_authService.CheckAuthorization(token) == false)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);

                if (NotesRepo.CheckForNote(id) == true)
                {
                    var note = (Note)NotesRepo.GetNote(id);
                    if (_authService.CurrentUser.Id == note.OwnerId)
                    {
                        NotesRepo.RemoveNote(id);
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }      
                    else
                        return Request.CreateResponse(HttpStatusCode.Forbidden);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch(Exception e)
            {
                WebApiApplication.GlobalLogger.Error(Request + e.ToString());
                HttpError err = new HttpError("Error while deleting note");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }
        }
    }
}
