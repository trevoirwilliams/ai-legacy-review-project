using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ClaimDesk.IntegrationTests;

public class ClaimsApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ClaimsApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetClaims_WithManagerHeaders_ShouldReturnSuccess()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-User-Id", "1");
        client.DefaultRequestHeaders.Add("X-User-Role", "Manager");

        var response = await client.GetAsync("/api/claims");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
