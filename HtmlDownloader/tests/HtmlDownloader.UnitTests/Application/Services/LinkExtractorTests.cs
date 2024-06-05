using Application.Interfaces;
using Application.Services;
using AutoFixture;
using Domain.ValueObjects;
using FluentAssertions;
using HtmlAgilityPack;
using Moq;

namespace HtmlDownloader.UnitTests.Application.Services;

public class LinkExtractorTests
{
    private readonly IFixture _fixture;

    public LinkExtractorTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public async Task The_page_associated_with_parent_link_is_downloaded_and_all_children_links_should_be_extracted()
    {
        var parentLink = new Link("https://www.example.com");

        var expectedChildrenLinks = new List<Link>()
        {
            new Link("https://www.example.com/page1"),
            new Link("https://www.example.com/page2"),
            new Link("https://www.example.com/page3")
        };

        var cancellationToken = _fixture.Create<CancellationToken>();

        HtmlDocument expectedHtmlDoc = CreateHtmlDocument();

        var htmlWebMock = new Mock<IHtmlWebWrapper>();
        htmlWebMock
            .Setup(x => x.LoadFromWebAsync(parentLink.UriString, cancellationToken))
            .ReturnsAsync(expectedHtmlDoc);

        var sut = new LinkExtractor(htmlWebMock.Object);

        var result = await sut.ExtractAsync(parentLink, cancellationToken);

        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expectedChildrenLinks);
    }

    private static HtmlDocument CreateHtmlDocument()
    {
        var html = @"
            <!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>HTML Example with Href Anchors</title>
            </head>
            <body>
                <h1>HTML Example with Href Anchors</h1>
    
                <ul>
                    <li><a href=""https://www.example.com/page1"">Page 1</a></li>
                    <li><a href=""https://www.example.com/page2"">Page 2</a></li>
                    <li><a href=""https://www.example.com/page3"">Page 3</a></li>
                </ul>
    
                <p>This is a simple HTML example with href anchors linking to actual URLs.</p>
    
            </body>
            </html>";

        var expectedHtmlDoc = new HtmlDocument();
        expectedHtmlDoc.LoadHtml(html);
        return expectedHtmlDoc;
    }
}
