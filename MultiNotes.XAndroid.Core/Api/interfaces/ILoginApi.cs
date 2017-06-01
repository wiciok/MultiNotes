using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MultiNotes.Model;

namespace MultiNotes.XAndroid.Core.Api
{
    public interface ILoginApi
    {
        bool Success { get; }
        string Message { get; }
        User User { get; }
        string Token { get; }

        void Login(string username, string password, bool hashed = false);
    }
}
