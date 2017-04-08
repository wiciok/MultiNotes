using MultiNotes.Core;
using MultiNotes.Server.Respositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MultiNotes.Server.Models;

namespace MultiNotes.Server.Controllers
{
    //todo: zrobic reroute!!!!

    public class TokenController : ApiController
    {
        private static readonly INoteRepository notesRepo = UnitOfWork.Instance.NotesRepository;
        private static readonly IUserRepository usersRepo = UnitOfWork.Instance.UsersRepository;
        private User user;

        private bool CheckAuthentication(string token)
        {
            Token tokenObj = TokenBase.CheckToken(token);
            if(tokenObj==null)
            {
                //throw new Exception("");
                return false;
            }
            //if(user==null) //??? czy to bedzie dzialac dla wielu uzytkownikow?
                user = usersRepo.GetUser(tokenObj.User.Id);
            return true;
        }


        // IQueryable zamiast IEnumerable - zwiększona wydajność.
        public IQueryable<Note> Get(string token)
        {
            if(CheckAuthentication(token)==true)
            {
                return notesRepo.GetAllNotes(user).AsQueryable();
            }

            //todo: jakas reakcja inna, np. rzucenie stanu http
            return null;
        }
    }
}
