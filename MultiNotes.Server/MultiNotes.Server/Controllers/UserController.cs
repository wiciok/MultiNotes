using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MultiNotes.Core;
using MultiNotes.Server.Respositories;

namespace MultiNotes.Server.Controllers
{
    public class UserController : ApiController
    {
        private static readonly IUserRepository usersRepo = UnitOfWork.Instance.UsersRepository;


        //todo: pozostale metody

        // POST api/note
        // public void Post([FromBody]Note value)
        public User Post(User value)
        {
            if (usersRepo.CheckForUser(value.Id) == false)
                return usersRepo.AddUser(value);
            else
                return null;
            //dac tutaj jakas odpowiedz http czy cos     
        }

    }
}
