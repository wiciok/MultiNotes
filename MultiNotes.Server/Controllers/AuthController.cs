using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net.Http;
using MultiNotes.Model;
using MultiNotes.Server.Services;

namespace MultiNotes.Server.Controllers
{
    [LogWebApiRequest]
    public class AuthController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork = UnitOfWork.Instance;

        private bool Authenticate(AuthenticationRecord authData)
        {
            User user;

            if (_unitOfWork.UsersRepository.CheckForUserByEmail(authData.Email) == false)
                return false;
            else
                user = _unitOfWork.UsersRepository.GetUserByEmail(authData.Email);

            return authData.PasswordHash == user.PasswordHash;
        }


        [ResponseType(typeof(string))]
        // POST api/auth
        [HttpPost]
        public HttpResponseMessage Post([FromBody]AuthenticationRecord userAuthData)
        {
            try
            {
                if (Authenticate(userAuthData)==true) //user istnieje, haslo sie zgadza
                {
                    var user = _unitOfWork.UsersRepository.GetUserByEmail(userAuthData.Email);

                    if(TokenBase.VerifyUserToken(user)==true) //jesli token juz istnieje  i jest ważny - zwracamy go
                    {
                        var token = TokenBase.GetUserToken(user);
                            return Request.CreateResponse(HttpStatusCode.OK, token.GetString);                            
                    }
                    //token nie istnieje - tworzymy go i zwracamy
                    return Request.CreateResponse(HttpStatusCode.OK, TokenBase.AddNewToken(user).GetString);
                }
                else
                {
                    HttpError err = new HttpError("Authentication Failed!");
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, err);
                }
            }
            catch(Exception)
            {
                HttpError err = new HttpError("Error while authentication");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }
        }
    }
}
