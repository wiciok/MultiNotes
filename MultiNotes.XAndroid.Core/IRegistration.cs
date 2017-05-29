﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiNotes.XAndroid.Core
{
    public interface IRegistration
    {
        bool IsRegisterSuccessful { get; }
        string RegisterMessage { get; }

        Task Register(string username, string password);
    }
}
