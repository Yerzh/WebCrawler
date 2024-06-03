namespace Domain.ValueObjects;

public record Link
{
    public Link(string uri)
    {
        UriString = uri;
        Uri = new Uri(uri, UriKind.RelativeOrAbsolute);
    }

    public Link(Uri baseUri, string relativeUri)
    {
        Uri = new Uri(baseUri, relativeUri);
        UriString = Uri.OriginalString;
    }

    public Uri Uri { get; private set; }

    public string UriString { get; private set; }
}
