using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiNotes.XAndroid.Core.Api
{
    internal interface IRegisterApi
    {
        bool IsRegisterSuccessful { get; }
        string RegisterMessage { get; }

        void Register(string username, string password);
    }
}
