namespace Ticaga.Application.Common;

internal static class ValidationHelper
{
    public static Dictionary<string, string[]> For(string field, string message)
    {
        return new()
        {
            [field] = [message]
        };
    }
}
