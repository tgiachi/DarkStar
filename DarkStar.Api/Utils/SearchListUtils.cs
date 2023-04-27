using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DarkStar.Api.Utils;
public static class SearchListUtils
{

    public static bool MatchesWildcard(string data, string wildCard)
    {
        var startsWith = "^";
        if (wildCard.StartsWith("*"))
        {
            wildCard = wildCard.Substring(1);
            startsWith = "^.*";
        }
        var pattern = $"{startsWith}{wildCard}";
        return Regex.IsMatch(data, pattern, RegexOptions.IgnoreCase);
    }
}
