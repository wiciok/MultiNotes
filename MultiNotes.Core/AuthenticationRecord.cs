﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiNotes.Core
{
    public class AuthenticationRecord
    {
        public string Login { get; set; }
        public string PasswordHash { get; set; }
    }
}
