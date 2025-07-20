using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using TicTacToe.Contracts.Moves;

namespace TicTacToe.Api.IntegrationTests.Controllers;

public class MovesApiIntegrationTests(IntegrationTestWebAppFactory factory)
    : IClassFixture<IntegrationTestWebAppFactory>
{
    private const string GamesEndpoint = "/api/v1/games";
    private const string MovesEndpointFormat = "/api/v1/games/{0}/moves";
    
    private readonly HttpClient _client = factory.CreateClient();
    
    private static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;
    
    private static string GetMovesEndpoint(int gameId) =>
        string.Format(InvariantCulture, MovesEndpointFormat, gameId);
    
    [Fact(DisplayName = "POST идемпотентность для одинаковых запросов")]
    public async Task PostMove_Should_return_ok_When_requests_concurrent()
    {
        // Arrange
        var postGameResponse = await _client.PostAsync(GamesEndpoint, null);
        postGameResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await postGameResponse.Content.ReadAsStringAsync();
        var gameId = JsonDocument.Parse(json).RootElement.GetInt32();

        var moveRequest = new CreateMoveRequest(0, 0);
        var movesEndpoint = GetMovesEndpoint(gameId);
        var idempotencyKey = Guid.NewGuid().ToString();
        
        using var request1 = new HttpRequestMessage(HttpMethod.Post, movesEndpoint);
        request1.Content = JsonContent.Create(moveRequest);
        request1.Headers.Add("X-Payout-Idempotency", idempotencyKey);

        using var request2 = new HttpRequestMessage(HttpMethod.Post, movesEndpoint);
        request2.Content = JsonContent.Create(moveRequest);
        request2.Headers.Add("X-Payout-Idempotency", idempotencyKey);

        // Act
        var responses = await Task.WhenAll(
            _client.SendAsync(request1, HttpCompletionOption.ResponseHeadersRead),
            _client.SendAsync(request2, HttpCompletionOption.ResponseHeadersRead)
        );

        // Assert
        responses[0].EnsureSuccessStatusCode();
        responses[1].EnsureSuccessStatusCode();
    }
}
