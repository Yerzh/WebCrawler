using Application.Services;
using Domain.ValueObjects;
using FluentAssertions;

namespace HtmlDownloader.UnitTests.Application.Services;

public class LinkFilterTests
{
    [Fact]
    public void Filters_out_links_with_schemes_other_than_http_and_https()
    {
        var links = new Link[]
        {
            new Link("http://leetcode.com/problemset"),
            new Link("mailto:someone@example.com"),
            new Link("tel:+4733378901"),
            new Link("/problemset/"),
            new Link("https://leetcode.com/problemset"),
            new Link("#section2"),
            new Link("javascript:alert('Hello World!');")
        };

        var filteredLinks = new Link[]
        {
            new Link("http://leetcode.com/problemset"),
            new Link("https://leetcode.com/problemset/"),
            new Link("https://leetcode.com/problemset")
        };

        CancellationToken cancellationToken = new CancellationToken();

        var sut = new LinkFilter();
        var actual = sut.Filter(links, "https://leetcode.com", cancellationToken);

        actual.Should().HaveCount(3);
        actual.Should().BeEquivalentTo(filteredLinks);
    }
}
