using MongoDB.Driver;
using MultiNotes.Server.Repositories;

namespace MultiNotes.Server.Services
{
    internal interface IUnitOfWork
    {
        string ConnectionString { get; }
        INoteRepository NotesRepository { get; }
        IUserRepository UsersRepository { get; }
        IMongoDatabase Database { get; }
    }

    //nie jest to moze do konca unit of work, ale przynajmniej przechowuje wszystkie repozytoria
    //singleton
    internal class UnitOfWork : IUnitOfWork
    {
        private static UnitOfWork _instance;
        public string ConnectionString => "mongodb://localhost:27017";

        public INoteRepository NotesRepository { get; }
        public IUserRepository UsersRepository { get; }
        public IMongoDatabase Database { get; }


        private UnitOfWork()
        {
            MongoClient client = new MongoClient(ConnectionString);
            Database = client.GetDatabase("MultiNotes");
            NotesRepository = new NoteRepository(Database);
            UsersRepository = new UserRepository(Database);
        }

        public static IUnitOfWork Instance => _instance ?? (_instance = new UnitOfWork());
    }
}