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
            //await a.register("nowiutki", "nowiutki");
            try
            {
                await a.login("nowiutki", "nowiutki");
            }
            catch(Exception e)
            {
                int asdasd = 2;
            }
            //await a.deleteAccount();
            //a.user.Name = "nowyname";
            NoteMethod test = new NoteMethod(ConnectionApi.httpClient);
            Note mojanotatka = new Note();
            mojanotatka.Id= await UniqueId.GetUniqueBsonId(ConnectionApi.httpClient);
            mojanotatka.Content = "aa";
            mojanotatka.OwnerId = a.user.Id;
            mojanotatka.LastChangeTimestamp = DateTime.Now;
            mojanotatka.CreateTimestamp = DateTime.Now;


            AuthenticationToken authenticationToken = new AuthenticationToken(ConnectionApi.httpClient);
            string token = await authenticationToken.PostAuthRecordAsync(a.Record);
            
            test.AddNote(mojanotatka,token);
            int aaaaa = 989;
        }
    }
}
