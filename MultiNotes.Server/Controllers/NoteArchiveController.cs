using MultiNotes.Server.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MultiNotes.Server.Models;
using System.Web.Http.Description;
using MultiNotes.Model;

namespace MultiNotes.Server
{
    [LogWebApiRequest]
    [RoutePrefix("api/notearchive")]
    public class NoteArchiveController : ApiController
    {
        private static readonly INoteArchiveRepository NotesArchivalRepo = UnitOfWork.Instance.NotesArchiveRepository;
        private static readonly AuthorizationService AuthService = new AuthorizationService(UnitOfWork.Instance.UsersRepository);

        //GET /api/notearchive/{token}
        //gets whole archive
        [Route("{token}")]
        [ResponseType(typeof(IQueryable<NoteArchival>))]
        [HttpGet]
        public HttpResponseMessage Get([FromUri] string token)
        {
            try
            {
                if (AuthService.CheckAuthorization(token) == true)
                {
                    return Request.CreateResponse<IQueryable<NoteArchival>>(HttpStatusCode.OK,
                        (IQueryable<NoteArchival>) NotesArchivalRepo.GetAllNotes(AuthService.currentUser).AsQueryable());
                }
                else
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            catch (Exception e)
            {
                WebApiApplication.GlobalLogger.Error(Request.ToString() + e.ToString());
                HttpError err = new HttpError("Error while getting all user's archival notes");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }
        }

        //GET /api/token/{token}/{id}
        //gets all versions of single note
        [Route("{token}/{id}")]
        [ResponseType(typeof(IEnumerable<NoteArchival>))]
        [HttpGet]
        public HttpResponseMessage Get([FromUri] string token, [FromUri] string id)
        {
            try
            {
                if (AuthService.CheckAuthorization(token) == false)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);

                if (NotesArchivalRepo.CheckForAnyVersionsOfNote(id) == false)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                IEnumerable<INote> noteArchivalList = NotesArchivalRepo.GetAllVersionsOfNote(id);

                //zbedna ta petla, ale powinna sie wykonać tylko raz, może jest jakis lepszy trick na to
                foreach (var el in noteArchivalList)
                {
                    if (el.OwnerId == AuthService.currentUser.Id)
                        return Request.CreateResponse<IQueryable<INote>>(HttpStatusCode.OK, noteArchivalList.AsQueryable());
                    else
                        return Request.CreateResponse(HttpStatusCode.Forbidden);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception e)
            {
                WebApiApplication.GlobalLogger.Error(Request.ToString() + e.ToString());
                HttpError err = new HttpError("Error while getting user archival notes - all versions of single note");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }
        }
    }
}
