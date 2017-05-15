using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace MultiNotes.Core
{
    class Mainn
    {
        public static int Main(string[] args)
        {
            try
            {
                Testy().Wait();
            }
            catch (Exception e)
            {
                string b = "sdsd";
            }
            Console.WriteLine("aaa");
            Console.ReadLine();
            return 0;
        }

        private static async Task Testy()
        {
            ConnectionApi.configure();
            UserMethod a = new UserMethod(ConnectionApi.httpClient);
            await a.register("nowiutki", "nowiutki");
            await a.login("nowiutki", "nowiutki");
            //await a.deleteAccount();
            //a.user.Name = "nowyname";
            NoteMethod test = new NoteMethod(ConnectionApi.httpClient);
            test.AddNote(a.user.Id, "notatka");
            int aaaaa = 989;
        }
    }
}
