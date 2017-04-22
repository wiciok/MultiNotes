using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            await a.registerAsync("aa", "bb");
        }
    }
}
