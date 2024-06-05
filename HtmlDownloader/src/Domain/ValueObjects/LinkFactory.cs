using Domain.Validators;

namespace Domain.ValueObjects;

public static class LinkFactory
{
    public static Link? Create(string uri)
    {
        bool valid = UriValidator.IsValid(uri);
        if (!valid)
        {
            return null;
        }

        return new Link(uri);
    }
}
