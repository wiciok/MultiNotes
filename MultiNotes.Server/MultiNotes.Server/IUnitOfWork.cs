using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiNotes.Server.Respositories;
using MongoDB.Driver;

namespace MultiNotes.Server
{
    interface IUnitOfWork
    {
        string ConnectionString { get; }
        INoteRepository NotesRepository { get; }
        IUserRepository UsersRepository { get; }
        IMongoDatabase Database { get; }
    }
}
