using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
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
        private readonly string _fileAuthenticationRecord = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MultiNotes", "user.dat");

        public UserMethod(HttpClient httpClient2)
        {
            _httpClient = httpClient2;
            _authenticationToken = new AuthenticationToken(_httpClient);
            Record = new AuthenticationRecord();

            string directoryName = Path.GetDirectoryName(_fileAuthenticationRecord);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
        }
        public bool PrepareAuthenticationRecord()
        {
            if (!File.Exists(_fileAuthenticationRecord))
                return false;

            var lines = File.ReadAllLines(_fileAuthenticationRecord);
            try
            {
                // ReSharper disable once ObjectCreationAsStatement
                new MailAddress(lines[0]);
            }
            catch (Exception e)
            {
                return false;
            }
            Record.Email = lines[0];
            Record.PasswordHash = lines[1];

            return true;
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
                File.WriteAllLines(_fileAuthenticationRecord, lines);
                PrepareAuthenticationRecord();
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
                //Conflict- loginow,InternalServerError
            }
        }
        public async Task Login(string email, string password, bool isPasswordHashed = false)
        {
            //logowanie do pliku
            var lines = isPasswordHashed ? new[] { email, password } : new[] { email, Encryption.Sha256(password) };
            File.WriteAllLines(_fileAuthenticationRecord, lines);
            PrepareAuthenticationRecord();

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
