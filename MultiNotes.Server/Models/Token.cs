using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MultiNotes.Core;


namespace MultiNotes.Server.Models
{
    class Token
    {
        public static readonly int expireTime = 200; //todo: zmniejszyc to, na razie jest tyle dla wygody

        private DateTime CreateTimestamp;
        public User User { get; }
        private string token;

        public string GetString
        {
            get { return token; }
        }

        public Token(User user)
        {
            this.CreateTimestamp = DateTime.Now;
            this.User = user;
            this.token = GenerateUniqueToken();
        }

        public bool IsValid
        {
            get
            {
                if ((DateTime.Now - CreateTimestamp).Seconds <= expireTime)
                    return true;
                else
                    return false;
            }
        }

        private static string GenerateUniqueToken()
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");

            //string GuidString = "testowytoken";

            return GuidString;
        }
    }
}