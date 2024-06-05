using Domain.ValueObjects;
using FluentAssertions;

namespace HtmlDownloader.UnitTests.Domain.ValueObjects;

public class LinkTests
{
    [Theory]
    [InlineData("https://leetcode.com/problems/")]
    [InlineData("http://leetcode.com")]
    public void Link_should_be_constructed_from_absolute_uri_string(string uriString)
    {
        var result = new Link(uriString);

        result.UriString.Should().Be(uriString);
        result.Uri.Should().Be(new Uri(uriString, UriKind.Absolute));
    }

    [Fact]
    public void Link_should_be_constructed_from_absolute_and_relative_uri_sections()
    {
        var absoluteUri = new Uri("https://leetcode.com");
        var relativeUriString = "/problems/";
        var expectedUri = new Uri(absoluteUri, relativeUriString);

        var result = new Link(absoluteUri, relativeUriString);

        result.UriString.Should().Be(expectedUri.OriginalString);
        result.Uri.Should().Be(expectedUri);
    }
}
