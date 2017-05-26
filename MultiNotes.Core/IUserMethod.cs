﻿using MultiNotes.Model;
using System.Threading.Tasks;

namespace MultiNotes.Core
{
    public interface IUserMethod
    {
        void PreparedAuthenticationRecord();
        Task register(string email, string password);
        Task login(string email, string password);
        Task<User> GetUserInfo(string token, string email);
        Task DeleteAccount();
        Task EditAccount();
        Task RemindPassword(string email);
    }
}
