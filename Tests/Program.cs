using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MultiNotes.Core;
using MultiNotes.Model;

namespace Tests
{
    class Program
    {
        public static async Task LoginTest()
        {
            var methods = new UserMethod(ConnectionApi.HttpClient);
            await methods.Login("g@g.com", "g");
            if (methods.User == null)
                throw new Exception();
            else
                Console.WriteLine(methods.User.EmailAddress, methods.User.Id);
        }

        public static async Task GetAllNotesTest()
        {
            var methods = new NoteMethod(ConnectionApi.HttpClient);
            var authtoken = new AuthenticationToken(ConnectionApi.HttpClient);
            var userMethod = new UserMethod(ConnectionApi.HttpClient);
            await userMethod.Login("s@s.com", "ss");
            var token=await authtoken.PostAuthRecordAsync(userMethod.Record);
            IEnumerable<Note> col = await methods.GetAllNotesFromDatabase(token);
            foreach (var el in col)
                Console.WriteLine(el.Id);
        }

        public static async Task PostNote()
        {
            var methods = new NoteMethod(ConnectionApi.HttpClient);
            var authtoken = new AuthenticationToken(ConnectionApi.HttpClient);
            var userMethod = new UserMethod(ConnectionApi.HttpClient);
            await userMethod.Login("s@s.com", "ss");
            var token = await authtoken.PostAuthRecordAsync(userMethod.Record);
            var id = await UniqueId.GetUniqueBsonId(ConnectionApi.HttpClient);
            var Note = new Note()
            {
                Content = "test",
                CreateTimestamp = DateTime.Now,
                Id = id,
                LastChangeTimestamp = DateTime.Now,
                OwnerId = userMethod.User.Id
            };
            methods.AddNoteToDatabase(Note, token);
        }

        static void Main(string[] args)
        {
            ConnectionApi.Configure();

            //GetAllNotesTest();
            PostNote();

            Console.ReadKey();
        }
    }
}
