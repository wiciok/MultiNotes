using System.Threading.Tasks;

using MultiNotes.Model;

namespace MultiNotes.XAndroid.Core
{
    public interface IUserApi
    {
        Task<User> GetUser(string token, string username);
    }
}
