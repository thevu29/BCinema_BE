namespace BCinema.Application.Utils;

public static class GenerateUtil
{
    public static string GenerateOtp()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }
}