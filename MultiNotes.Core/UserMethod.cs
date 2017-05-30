using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Web.Http;
using MultiNotes.Model;

namespace MultiNotes.Core
{
    public class UserMethod : IUserMethod
    {
        private static HttpClient _httpClient;
        private readonly AuthenticationToken _authenticationToken;

        public AuthenticationRecord Record { get; set; }
        public User User;
        private readonly string FileAuthenticationRecord = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MultiNotes", "user.dat");

        public UserMethod(HttpClient httpClient2)
        {
            _httpClient = httpClient2;
            _authenticationToken = new AuthenticationToken(_httpClient);
            Record = new AuthenticationRecord();

            string directoryName = System.IO.Path.GetDirectoryName(FileAuthenticationRecord);
            if (!System.IO.Directory.Exists(directoryName))
            {
                System.IO.Directory.CreateDirectory(directoryName);
            }
        }
        public void PreparedAuthenticationRecord()
        {
            var lines = System.IO.File.ReadAllLines(FileAuthenticationRecord);
            Record.Email = lines[0];
            Record.PasswordHash = lines[1];
        }
        public async Task Register(string email, string password)
        {
            var bsonId = await UniqueId.GetUniqueBsonId(_httpClient);
            var passwordHash = Encryption.Sha256(password);
            User = new User
            {
                Id = bsonId,
                EmailAddress = email,
                PasswordHash = passwordHash,
                RegistrationTimestamp = DateTime.Now
            };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/user", User);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                string[] lines = { email, passwordHash };
                System.IO.File.WriteAllLines(FileAuthenticationRecord, lines);
                PreparedAuthenticationRecord();
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
                //Conflict- loginow,InternalServerError
            }
        }
        public async Task Login(string email, string password)
        {
            //logowanie do pliku
            string[] lines = { email, Encryption.Sha256(password) };
            System.IO.File.WriteAllLines(FileAuthenticationRecord, lines);
            PreparedAuthenticationRecord();

            //wypelnienie uzytkownika
            var token = await _authenticationToken.PostAuthRecordAsync(Record);
            User = await GetUserInfo(token, Record.Email);
        }

        public async Task<User> GetUserInfo(string token, string email)
        {
            email = Encryption.Base64Encode(email);

            User user;
            var response = _httpClient.GetAsync("api/user/" + token + "/" + email).Result;
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
            var token = await _authenticationToken.PostAuthRecordAsync(Record);
            var user = GetUserInfo(token, Record.Email);
            var response = await _httpClient.DeleteAsync("api/user/" + token + "/" + user.Result.Id);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpResponseException(response.StatusCode);
                //Forbidden,Unauthorized,InternalServerError
            }
        }

        public async Task EditAccount()
        {
            var token = await _authenticationToken.PostAuthRecordAsync(Record);
            var user = GetUserInfo(token, Record.Email);
            var response = await _httpClient.PutAsJsonAsync("api/user/" + token, user);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpResponseException(response.StatusCode);
                //Forbidden,Unauthorized,InternalServerError
            }
        }

        public async Task RemindPassword(string email)
        {
            email = Encryption.Base64Encode(email);

            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, "api/ResetPassword/" + email);
            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpResponseException(response.StatusCode);
                //NotFound,InternalServerError
            }
        }
    }
}
