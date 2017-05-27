using MongoDB.Driver;
using System.Collections.Generic;
using MultiNotes.Model;

namespace MultiNotes.Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserRepository(IMongoDatabase database)
        {
            _usersCollection = database.GetCollection<User>("Users");
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _usersCollection.Find(n => true).ToList();
        }

        public User GetUser(string id)
        {
            return _usersCollection.Find(n => n.Id == id).Single<User>();
        }

        public User GetUserByEmail(string email)
        {
            return _usersCollection.Find(n => n.EmailAddress == email).Single<User>();
        }

        public bool CheckForUser(string id)
        {
            return _usersCollection.Count(n => n.Id == id) != 0;
        }

        public bool CheckForUserByEmail(string email)
        {
            return _usersCollection.Count(n => n.EmailAddress == email) != 0;
        }

        public User AddUser(User user)
        {   _usersCollection.InsertOne(user);
            return user;
        }

        public void RemoveUser(string id)
        {
            _usersCollection.DeleteOne(n => n.Id == id);
        }

        public void UpdateUser(string id, User user)
        {
            _usersCollection.FindOneAndReplace(b => b.Id == id, user);
        }
    }
}