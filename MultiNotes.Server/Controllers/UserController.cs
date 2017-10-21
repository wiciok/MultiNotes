using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MultiNotes.Server.Repositories;
using System.Web.Http.Description;
using MultiNotes.Model;
using MultiNotes.Server.Services;

namespace MultiNotes.Server.Controllers
{
    [LogWebApiRequest]
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private static readonly IUserRepository UsersRepo = UnitOfWork.Instance.UsersRepository;
        private readonly AuthorizationService _authService = new AuthorizationService(UsersRepo);

        //pobieranie danych uzytkownika np. po zalogowaniu w kliencie
        // GET api/user/32q2fdrsdfa/5
        [Route("{token}/{email}")]
        [ResponseType(typeof(User))]
        [HttpGet]
        public HttpResponseMessage Get(string token, string email)
        {
            email = Encryption.Base64Decode(email);
            try
            {
                if (_authService.CheckAuthorization(token) == true)
                {
                    return _authService.CurrentUser.EmailAddress == email 
                        ? Request.CreateResponse(HttpStatusCode.OK, UsersRepo.GetUserByEmail(email))
                        : Request.CreateResponse(HttpStatusCode.Forbidden);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            catch (Exception e)
            {
                WebApiApplication.GlobalLogger.Error(Request + e.ToString());
                var err = new HttpError("Error while getting user");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }
        }

        //rejestracja nowego uzytkownika
        // POST api/user
        [ResponseType(typeof(User))]
        [HttpPost]
        public HttpResponseMessage Post([FromBody]User user)
        {
            try
            {
                if (UsersRepo.CheckForUser(user.Id) == false &&
                    UsersRepo.CheckForUserByEmail(user.EmailAddress) == false)
                {
                    //email validation
                    try
                    {
                        new System.Net.Mail.MailAddress(user.EmailAddress);
                    }
                    catch
                    {
                        var err = new HttpError("Bad email address format!");
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }

                    return Request.CreateResponse(HttpStatusCode.Created, UsersRepo.AddUser(user));
                }
                    
                else
                {
                    var err = new HttpError("User already exists!");
                    return Request.CreateResponse(HttpStatusCode.Conflict, err);
                }
            }
            catch (Exception e)
            {
                WebApiApplication.GlobalLogger.Error(Request + e.ToString());
                var err = new HttpError("Error while creating user");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }
        }


        // edycja istniejacego uzytkownika
        // PUT api/user/32q2fdrsdfa
        [Route("{token}")]
        [ResponseType(typeof(User))]
        [HttpPut]
        public HttpResponseMessage Put([FromUri]string token, [FromBody]User value)
        {
            try
            {
                if (_authService.CheckAuthorization(token) == true)
                {
                    if (_authService.CurrentUser.Id != value.Id)
                        return Request.CreateResponse(HttpStatusCode.Forbidden); //probojemy zmienic dane innego uzytkownika niz my sami

                    else
                    {
                        UsersRepo.UpdateUser(value.Id, value);
                        return Request.CreateResponse(HttpStatusCode.OK, value);
                    }
                }
                else
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            catch (Exception e)
            {
                WebApiApplication.GlobalLogger.Error(Request + e.ToString());
                var err = new HttpError("Error while editing user");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }
        }

        // usuniecie istniejacego uzytkownika (siebie)
        // DELETE api/user/32q2fdrsdfa
        [Route("{token}/{id_user}")]
        [ResponseType(typeof(User))]
        [HttpDelete]
        public HttpResponseMessage Delete([FromUri]string token, [FromUri]string id_user)
        {
            try
            {
                if (_authService.CheckAuthorization(token) == true)
                {
                    if (_authService.CurrentUser.Id != id_user)
                    {
                        return Request.CreateResponse(HttpStatusCode.Forbidden); //probojemy usunac innego uzytkownika niz my sami
                    }
                    else
                    {
                        UsersRepo.RemoveUser(id_user);
                        TokenBase.RemoveToken(token); //usunelismy sie = tracimy autentykację
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
                else
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            catch (Exception e)
            {
                WebApiApplication.GlobalLogger.Error(Request + e.ToString());
                var err = new HttpError("Error while removing user");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }
        }

        //sends email with user password reset link
        [Route("{email}")]
        [ResponseType(typeof(User))]
        [HttpPatch]
        public HttpResponseMessage Patch([FromUri]string email)
        {
            email = Encryption.Base64Decode(email);
            try
            {
                if (UsersRepo.CheckForUserByEmail(email))
                {
                    PasswordResetService.Instance.SendEmailWithToken(email);   
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch (Exception e)
            {
                WebApiApplication.GlobalLogger.Error(Request + e.ToString());
                var err = new HttpError("Error while sending user password reset link!");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }
        }
    }
}
