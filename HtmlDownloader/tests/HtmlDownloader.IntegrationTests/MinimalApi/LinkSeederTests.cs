using Api.Dtos;
using HtmlDownloader.IntegrationTests.Helpers;
using System.Net.Http.Json;

namespace HtmlDownloader.IntegrationTests.MinimalApi;

public class LinkSeederTests : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public LinkSeederTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Seed_with_download_link()
    {
        var client = _factory.CreateClient();

        DownloadLinkRequest request = new()
        {
            Type = "https://leetcode.com",
            Uri = "https://leetcode.com/problemset/"
        };

        var content = JsonContent.Create(request);
        var response = await client.PostAsync("/seed", content);

        response.EnsureSuccessStatusCode();
    }

    public async Task InitializeAsync()
    {
        await _factory.RedisContainer.StartAsync().ConfigureAwait(false);
    }

    public async Task DisposeAsync()
    {
        await _factory.RedisContainer.DisposeAsync();
    }
}
