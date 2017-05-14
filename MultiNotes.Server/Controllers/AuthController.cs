using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using MultiNotes.Core;
using MultiNotes.Server;
using MultiNotes.Server.Models;
using System.Web.Http.Description;
using System.Net.Http;

namespace MultiNotes.Server.Controllers
{
    [LogWebApiRequest]
    public class AuthController : ApiController
    {
        private IUnitOfWork unitOfWork = UnitOfWork.Instance;

        private bool Authenticate(AuthenticationRecord authData)
        {
            User user;

            if (unitOfWork.UsersRepository.CheckForUserByLogin(authData.Login) == false)
                return false;
            else
                user = unitOfWork.UsersRepository.GetUserByLogin(authData.Login);

            if (authData.PasswordHash == user.PasswordHash)
                return true;
            else
                return false;
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
                    User user = unitOfWork.UsersRepository.GetUserByLogin(userAuthData.Login);

                    if(TokenBase.VerifyUserToken(user)==true) //jesli token juz istnieje  i jest ważny - zwracamy go
                    {
                        Token token = TokenBase.GetUserToken(user);
                            return Request.CreateResponse<string>(HttpStatusCode.OK, token.GetString);                            
                    }
                    //token nie istnieje - tworzymy go i zwracamy
                    return Request.CreateResponse<string>(HttpStatusCode.OK, TokenBase.AddNewToken(user).GetString);
                }
                else
                {
                    HttpError err = new HttpError("Authentication Failed!");
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, err);
                }
            }
            catch(Exception e)
            {
                HttpError err = new HttpError("Error while authentication");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }
        }
    }
}
