using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MultiNotes.Model;

namespace MultiNotes.XAndroid.Core.Api
{
    public interface IAuthTokenApi
    {
        string GetAuthToken(AuthenticationRecord record);
    }
}