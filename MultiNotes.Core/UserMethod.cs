using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Web.Http;
using MultiNotes.Model;

namespace MultiNotes.Core
{
    public class UserMethod: IUserMethod
    {
        private static HttpClient httpClient;
        private AuthenticationToken authenticationToken;
        public AuthenticationRecord Record { get; set; }
        public User user;
        private string fileAuthenticationRecord = "user.txt";
        public UserMethod(HttpClient httpClient2)
        {
            httpClient = httpClient2;
            authenticationToken = new AuthenticationToken(httpClient);
            Record = new AuthenticationRecord();
            
        }
        public void PreparedAuthenticationRecord()
        {
            string[] lines = System.IO.File.ReadAllLines(fileAuthenticationRecord);
            Record.Email = lines[0];
            Record.PasswordHash = lines[1];
        }
        public async Task register(string email, string password)
        {
            string BsonId = null;

            BsonId = await UniqueId.GetUniqueBsonId(httpClient);
            string passwordHash = Encryption.Sha256(password);
            user = new User(BsonId, passwordHash,email);

            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/user", user);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                string[] lines = { email, passwordHash };
                System.IO.File.WriteAllLines("users.txt", lines);
                PreparedAuthenticationRecord();
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
                //Conflict- loginow,InternalServerError
            }
        }
        public async Task login(string email, string password)
        {
            //logowanie do pliku
            string[] lines = { email, Encryption.Sha256(password) };
            System.IO.File.WriteAllLines("plik.txt", lines);
            PreparedAuthenticationRecord();

            //wypelnienie uzytkownika
            string token = await authenticationToken.PostAuthRecordAsync(Record);
            user = await GetUserInfo(token, Record.Email);
        }

        public async Task<User> GetUserInfo(string token,string email)
        {
            User user = null;
            HttpResponseMessage response = await httpClient.GetAsync("api/user/" + token + "/" + email);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                user = await response.Content.ReadAsAsync<User>();
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
                //Forbidden,Unauthorized,InternalServerError,BadRequest- niepoprawny email
            }
            return user;
        }

        public async Task DeleteAccount()
        {
            string token = await authenticationToken.PostAuthRecordAsync(Record);
            var user = GetUserInfo(token, Record.Email);
            HttpResponseMessage response = await httpClient.DeleteAsync("api/user/" + token + "/" + user.Result.Id);

            if(response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpResponseException(response.StatusCode);
                //Forbidden,Unauthorized,InternalServerError
            }
        }

        public async Task EditAccount()
        {
            string token = await authenticationToken.PostAuthRecordAsync(Record);
            var user = GetUserInfo(token, Record.Email);
            HttpResponseMessage response = await httpClient.PutAsJsonAsync("api/user/"+token, user);

            if(response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpResponseException(response.StatusCode);
                //Forbidden,Unauthorized,InternalServerError
            }
        }

        public async Task RemindPassword(string email)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, "api/ResetPassword/"+email);
            HttpResponseMessage response = new HttpResponseMessage();
            response = await httpClient.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpResponseException(response.StatusCode);
                //NotFound,InternalServerError
            }
        }
    }
}
