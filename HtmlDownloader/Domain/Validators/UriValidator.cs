namespace Domain.Validators;

public static class UriValidator
{
    public static bool IsValid(string uri)
    {
        return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
    }
}
