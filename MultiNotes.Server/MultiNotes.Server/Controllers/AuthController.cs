using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MultiNotes.Core;
using MultiNotes.Server;
using MultiNotes.Server.Models;

namespace MultiNotes.Server.Controllers
{
    public class AuthController : ApiController
    {
        private IUnitOfWork unitOfWork = UnitOfWork.Instance;

        // POST api/auth
        public string Post([FromBody]AuthenticationRecord userAuthData)
        {
            bool isAuthSuccessful=AuthenticationService.Authenticate(userAuthData);

            if(isAuthSuccessful)
            {
                User user = unitOfWork.UsersRepository.GetUser(userAuthData.UserId);
                return TokenBase.AddNewToken(user).ToString();
            }
            else
            {
                //tutaj jakas obsluga wyjatkow?
                return "";
            }
        }
    }
}
