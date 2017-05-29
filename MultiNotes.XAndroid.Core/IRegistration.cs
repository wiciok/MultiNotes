using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MultiNotes.XAndroid.Core
{
    public interface IRegistration
    {
        bool IsRegisterSuccessful { get; }
        string RegisterMessage { get; }

        Task Register(string username, string password);
    }
}
