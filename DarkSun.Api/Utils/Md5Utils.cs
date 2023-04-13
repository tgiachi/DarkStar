using System.Security.Cryptography;
using System.Text;

namespace DarkSun.Api.Utils;

public static class Md5Utils
{
    public static string CreateMd5Hash(this string input)
    {
        var md5 = MD5.Create();
        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hashBytes = md5.ComputeHash(inputBytes);

        var sb = new StringBuilder();
        foreach (var t in hashBytes)
        {
            sb.Append(t.ToString("X2"));
        }

        return sb.ToString();
    }
}
