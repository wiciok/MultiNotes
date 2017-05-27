using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MultiNotes.Model
{
    public class Encryption
    {
        public static string Sha256(string text)
        {
            var crypt = new SHA256Managed();
            var hash = string.Empty;
            var crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(text), 0, Encoding.ASCII.GetByteCount(text));
            return crypto.Aggregate(hash, (current, theByte) => current + theByte.ToString("x2"));
        }
    }
}
