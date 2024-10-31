namespace BCinema.Application.Utils;

public static class StringUtil
{
    public static string UppercaseFirstLetter(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return char.ToUpper(input[0]) + input[1..].ToLower();
    }
}
