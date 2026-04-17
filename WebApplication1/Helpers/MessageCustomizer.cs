namespace WebApplication1.Helpers;

public static class MessageCustomizer
{
    public static string Customize(this string message)
    {
        return $"Custom message: {message}";
    }
}