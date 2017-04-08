using MultiNotes.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MultiNotes.Server.Respositories
{
    interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUser(string id);
        User AddUser(User item);
        bool RemoveUser(string id);
        bool UpdateUser(string id, User item);
        bool CheckForUser(string id);
    }
}
