using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MultiNotes.Server.Repositories;
using MongoDB.Driver;

namespace MultiNotes.Server
{
    //nie jest to moze do konca unit of work, ale przynajmniej przechowuje wszystkie repozytoria
    //singleton
    class UnitOfWork : IUnitOfWork
    {
        private static UnitOfWork instance;
        private static readonly string connectionString = "mongodb://localhost:27017";
        private MongoClient client;

        public string ConnectionString { get { return connectionString; } }

        public INoteRepository NotesRepository { get; }
        public IUserRepository UsersRepository { get; }
        public IMongoDatabase Database { get; }


        private UnitOfWork()
        {
            this.client = new MongoClient(ConnectionString);
            this.Database = client.GetDatabase("MultiNotes");
            this.NotesRepository = new NoteRepository(Database);
            this.UsersRepository = new UserRepository(Database);
        }

        public static IUnitOfWork Instance
        {
            get
            {
                if (instance == null)
                    instance = new UnitOfWork();
                return instance;
            }
        }
    }
}