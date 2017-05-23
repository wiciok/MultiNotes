using System;
using MultiNotes.Model;
using MultiNotes.Server.Services;


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
            this.token = RandomTokenGenerator.GenerateUniqueToken();
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
    }
}