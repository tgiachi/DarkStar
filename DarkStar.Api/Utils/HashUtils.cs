using System.Security.Cryptography;
using System.Text;

namespace DarkStar.Api.Utils;

public static class HashUtils
{
    public static string CreateBCryptHash(this string input) => BCrypt.Net.BCrypt.HashPassword(input);

    public static string Sha1Hash(this string input) => Convert.ToHexString(SHA1.HashData((Encoding.UTF8.GetBytes(input))));
}
