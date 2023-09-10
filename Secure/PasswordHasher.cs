using System.Security.Cryptography;
using System.Text;

namespace Game2048.Secure
{
    public static class PasswordHasher
    {
        public static string Hash(string data)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            var bytes = Encoding.Unicode.GetBytes(data);
            var sha1data = sha1.ComputeHash(bytes);
            var hashedPassword = Encoding.Unicode.GetString(sha1data);
            return hashedPassword;
        }
    }
}