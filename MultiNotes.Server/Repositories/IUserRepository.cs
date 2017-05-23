using System.Collections.Generic;
using MultiNotes.Model;


namespace MultiNotes.Server.Repositories
{
    interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUser(string id);
        User GetUserByEmail(string email);
        User AddUser(User item);
        void RemoveUser(string id);
        void UpdateUser(string id, User item);
        bool CheckForUser(string id);
        bool CheckForUserByEmail(string email);
    }
}
