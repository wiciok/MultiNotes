using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MultiNotes.Core;


namespace MultiNotes.Server.Models
{
    class Token
    {
        public static readonly int expireTime = 30;

        private DateTime CreateTimestamp;
        public User User { get; }

        public string GetToken
        {
            get { return GenerateUniqueToken(); }
        }

        public Token(User user)
        {
            this.CreateTimestamp = DateTime.Now;
            this.User = user;
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

            return GuidString;
        }
    }
}