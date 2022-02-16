using Manganese.Text;

namespace AHpx.RG.Core.Utils;

/// <summary>
/// move to manganese
/// </summary>
public static class StringUtils
{
    public static string CapitalizeInitial(this string origin)
    {
        if (origin.IsNullOrEmpty())
            return origin;

        return string.Concat(origin[0].ToString().ToUpper(), origin.AsSpan(1));
    }
}