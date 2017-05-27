using MultiNotes.Model;
using MultiNotes.Server.Repositories;

namespace MultiNotes.Server.Services
{
    class AuthorizationService
    {
        public User CurrentUser;
        private readonly IUserRepository _userRepo;

        public AuthorizationService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public bool CheckAuthorization(string token)
        {

            var tokenObj = TokenBase.GetToken(token);
            if (tokenObj == null)
                return false;
            CurrentUser = _userRepo.GetUser(tokenObj.User.Id);
            return true;
        }
    }
}