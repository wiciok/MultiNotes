using MultiNotes.Core;
using MultiNotes.Server.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MultiNotes.Server.Models;

namespace MultiNotes.Server
{
    //todo: zrobic reroute!!!!

    [RoutePrefix("api/token")]
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
        [Route("{token}")] //bez tego ni uja nie bedzie dzialac, mimo ze w klase NotesController jest tak samo i działa
        public IQueryable<Note> Get([FromUri]string token)
        {
            if(CheckAuthentication(token)==true)
            {
                return notesRepo.GetAllNotes(user).AsQueryable();
            }

            //todo: jakas reakcja inna, np. rzucenie stanu http
            return null;
        }

        //tutaj sytuacja jak wyżej, ael prostsza metoda do testu
        //nie wiem jak zrobić żeby się to wywoływało, pewnie trzeba w routes pogrzebać
        //zamiast tej, wywołuje się metoda niżej (bez parametru) o ile nie jest wykomentowana

        /*[Route("{token}")]
        public string Get([FromUri]string token)
        {
            return "testRetzargumentem";
        }


         [Route("")]
        public string Get()
        {
            return "testRetbezargumentu";
        }*/


        //GET /api/token/{token}/{id}
        [Route("{token}/{id}")]
        public Note Get(string token, string id)
        {
            //calosc jest okropna, zmienic to potem

            if (CheckAuthentication(token) == false)
                return null;

            Note note;
            try
            {
               note  = notesRepo.GetNote(id);
            }
            catch(InvalidOperationException)
            {
                return null;
            }

            if (note.OwnerId == user.Id)
                return note;
            else
                return null;
        }


        /*
        // POST api/note
        // public void Post([FromBody]Note value)
        public Note Post(string token, [FromBody]Note value)
        {
            if (CheckAuthentication(token) == true)
            {
                

                return notesRepo.GetNote()
            }
        }*/

    }
}
