using System;
using MultiNotes.Model;

namespace MultiNotes.Server.Services
{
    class Token
    {
        public static readonly int ExpireTime = 200; //todo: zmniejszyc to, na razie jest tyle dla wygody

        private readonly DateTime _createTimestamp;
        public User User { get; }
        public string GetString { get; }

        public Token(User user)
        {
            _createTimestamp = DateTime.Now;
            User = user;
            GetString = RandomTokenGenerator.GenerateUniqueToken();
        }

        public bool IsValid => (DateTime.Now - _createTimestamp).Seconds <= ExpireTime;
    }
}