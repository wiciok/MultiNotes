using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MultiNotes.Core;
using MultiNotes.Server.Repositories;
using System.Web.Http.Description;
using MultiNotes.Server.Models;

namespace MultiNotes.Server.Controllers
{
    [LogWebApiRequest]
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private static readonly IUserRepository usersRepo = UnitOfWork.Instance.UsersRepository;
        private AuthorizationService authService = new AuthorizationService(usersRepo);

        //pobieranie danych uzytkownika np. po zalogowaniu w kliencie
        // GET api/user/32q2fdrsdfa/5
        [Route("{token}/{login}")]
        [ResponseType(typeof(User))]
        [HttpGet]
        public HttpResponseMessage Get(string token, string login)
        {
            try
            {
                if (authService.CheckAuthorization(token) == true)
                {
                    if(authService.currentUser.Login==login)
                        return Request.CreateResponse<User>(HttpStatusCode.OK, usersRepo.GetUserByLogin(login));
                    else
                        return Request.CreateResponse(HttpStatusCode.Forbidden); //probojemy pobrac dane innego uzytkownika niz my sami
                }
                else
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            catch(Exception e)
            {
                WebApiApplication.GlobalLogger.Error(Request.ToString() + e.ToString());
                HttpError err = new HttpError("Error while getting user");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }
        }


        //rejestracja nowego uzytkownika
        // POST api/user
        //[Route("{user}")]
        [ResponseType(typeof(User))]
        [HttpPost]
        public HttpResponseMessage Post([FromBody]User user)
        {
            try
            {
                if (usersRepo.CheckForUser(user.Id) == false && usersRepo.CheckForUserByLogin(user.Login)==false)
                    return Request.CreateResponse<User>(HttpStatusCode.Created, usersRepo.AddUser(user));
                else
                {
                    HttpError err = new HttpError("User already exists!");
                    return Request.CreateResponse(HttpStatusCode.Conflict, err);
                }
            }
            catch(Exception e)
            {
                WebApiApplication.GlobalLogger.Error(Request.ToString() + e.ToString());
                HttpError err = new HttpError("Error while creating user");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }
        }


        // edycja istniejacego uzytkownika
        // PUT api/user/32q2fdrsdfa
        //[Route("{token}/{value}")]
        [Route("{token}")]
        [ResponseType(typeof(User))]
        [HttpPut]
        public HttpResponseMessage Put([FromUri]string token, [FromBody]User value)
        {
            try
            {
                if (authService.CheckAuthorization(token) == true)
                {
                    if(authService.currentUser.Id!=value.Id)
                        return Request.CreateResponse(HttpStatusCode.Forbidden); //probojemy zmienic dane innego uzytkownika niz my sami

                    else
                    {
                        usersRepo.UpdateUser(value.Id, value);
                        return Request.CreateResponse(HttpStatusCode.OK, value);
                    }
                }
                else
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            catch(Exception e)
            {
                WebApiApplication.GlobalLogger.Error(Request.ToString() + e.ToString());
                HttpError err = new HttpError("Error while editing user");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }
        }

        // usuniecie istniejacego uzytkownika (siebie)
        // DELETE api/user/32q2fdrsdfa
        //[Route("{token}/{value}")]
        [Route("{token}/{id_user}")]
        [ResponseType(typeof(User))]
        [HttpDelete]
        public HttpResponseMessage Delete([FromUri]string token, [FromUri]string id_user)
        {
            try
            {
                if (authService.CheckAuthorization(token) == true)
                {
                    if (authService.currentUser.Id != id_user)
                    {
                        return Request.CreateResponse(HttpStatusCode.Forbidden); //probojemy usunac innego uzytkownika niz my sami
                    }
                    else
                    {
                        usersRepo.RemoveUser(id_user);
                        TokenBase.RemoveToken(token); //usunelismy sie = tracimy autentykację
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
                else
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            catch(Exception e)
            {
                WebApiApplication.GlobalLogger.Error(Request.ToString() + e.ToString());
                HttpError err = new HttpError("Error while removing user");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }
        }
    }
}
