using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MultiNotes.Model;

namespace MultiNotes.XAndroid.Core
{
    public interface ILoginEngine
    {
        bool IsLoginSuccessful { get; }
        string LoginMessage { get; }
        User User { get; }
        string Token { get; }

        Task Login(string username, string password, bool hashed = false);
    }
}
