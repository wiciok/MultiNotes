using MultiNotes.Model;
using System.Threading.Tasks;

namespace MultiNotes.Core
{
    public interface IUserMethod
    {
        void PreparedAuthenticationRecord();
        Task Register(string email, string password);
        Task Login(string email, string password, bool isPasswordHashed);
        Task<User> GetUserInfo(string token, string email);
        Task DeleteAccount();
        Task EditAccount();
        Task RemindPassword(string email);
    }
}
