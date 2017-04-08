using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MultiNotes.Core;

namespace MultiNotes.Server.Models
{
    public static class AuthenticationService
    {
        public static bool Authenticate(AuthenticationRecord authData)
        {
            IUnitOfWork unitOfWork = UnitOfWork.Instance;
            User user = unitOfWork.UsersRepository.GetUser(authData.UserId);
            if (user == null)
            {
                //throw new Exception("User with specified id doesn't exist");
                //todo: sensowna obsluga wyjatkow
                return false;
            }

            else
            {
                if (authData.PasswordHash == user.PasswordHash)
                    return true;
                else
                    return false;
            }
        }
    }
}