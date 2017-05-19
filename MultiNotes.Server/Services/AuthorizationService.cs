using MultiNotes.Server.Repositories;
using MultiNotes.Model;

namespace MultiNotes.Server.Models
{
    class AuthorizationService
    {
        public User currentUser;
        private IUserRepository userRepo;

        public AuthorizationService(IUserRepository userRepo)
        {
            this.userRepo = userRepo;
        }

        public bool CheckAuthorization(string token)
        {

            Token tokenObj = TokenBase.GetToken(token);
            if (tokenObj == null)
            {
                return false;
            }
            currentUser = userRepo.GetUser(tokenObj.User.Id);
            return true;
        }
    }
}