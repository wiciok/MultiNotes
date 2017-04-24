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
            //await a.registerAsync("aa", "bb");
            // a.login("aa", "bb");
            //await a.deleteAccount();
            //a.user.Name = "nowyname";
            try
            {
                await a.editAccount();
            }
            catch(HttpResponseException e)
            {
                if(e.Response.StatusCode== HttpStatusCode.NotFound)
                {
                    //jezeli masz not found to zrob to
                }
                else
                {
                    //inne wyjatki
                }
            }
        }
    }
}
