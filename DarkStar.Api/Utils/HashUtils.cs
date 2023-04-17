using System.Security.Cryptography;
using System.Text;

namespace DarkStar.Api.Utils;

public static class HashUtils
{
    public static string CreateBCryptHash(this string input)
    {
        return BCrypt.Net.BCrypt.HashPassword(input);
    }
}
