using Domain.ValueObjects;
using FluentAssertions;

namespace HtmlDownloader.UnitTests.Domain.ValueObjects;

public class LinkFactoryTests
{
    [Theory]
    [InlineData("https://leetcode.com/problems/")]
    [InlineData("http://leetcode.com")]
    public void Link_should_be_created_from_valid_uri_string(string uriString)
    {
        var expectedLink = new Link(uriString);

        var result = LinkFactory.Create(uriString);

        result.Should().NotBeNull();
        result.Should().Be(expectedLink);
    }

    [Fact]
    public void Null_should_be_retrieved_when_passing_invalid_uri_string()
    {
        var uriString = "https:///leetcode.com/problems/";

        var result = LinkFactory.Create(uriString);

        result.Should().BeNull();
    }
}
