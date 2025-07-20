using System.Net;
using System.Text.Json;
using FluentAssertions;

namespace TicTacToe.Api.IntegrationTests.Controllers;

public class GamesApiIntegrationTests(IntegrationTestWebAppFactory factory)
    : IClassFixture<IntegrationTestWebAppFactory>
{
    private const string AnyNumberRegex = @"\d+";
    private const string GamesEndpoint = "/api/v1/games";
    
    private readonly HttpClient _client = factory.CreateClient();
    
    [Fact(DisplayName = "POST успешно создает 1 игру")]
    public async Task Post_Should_create_game()
    {
        // Act
        var response = await _client.PostAsync(GamesEndpoint, null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var json = await response.Content.ReadAsStringAsync();
        json.Should().MatchRegex(AnyNumberRegex);
    }
    
    [Fact(DisplayName = "GET возвращает игру по id")]
    public async Task Get_Should_return_game_by_id()
    {
        // Arrange
        var postResponse = await _client.PostAsync(GamesEndpoint, null);
        postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var postContent = await postResponse.Content.ReadAsStringAsync();
    
        using var jsonDoc = JsonDocument.Parse(postContent);
        var id = jsonDoc.RootElement.GetInt32();

        // Act
        var getResponse = await _client.GetAsync($"{GamesEndpoint}/{id}");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var getContent = await getResponse.Content.ReadAsStringAsync();
        getContent.Should().Contain($"\"id\":{id}");
    }
}
