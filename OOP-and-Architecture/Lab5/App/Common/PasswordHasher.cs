using System.Security.Cryptography;
using System.Text;

namespace Lab5.App.Common;

public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(password);
        byte[] hash = SHA256.HashData(bytes);

        return Convert.ToHexString(hash);
    }

    public static bool VerifyHashedPassword(string hash, string password)
    {
        string passwordHash = HashPassword(password);
        
        return passwordHash == hash;
    }
}