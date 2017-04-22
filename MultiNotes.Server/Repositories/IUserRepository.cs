﻿using MultiNotes.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MultiNotes.Server.Repositories
{
    interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUser(string id);
        User AddUser(User item);
        void RemoveUser(string id);
        void UpdateUser(string id, User item);
        bool CheckForUser(string id);
    }
}