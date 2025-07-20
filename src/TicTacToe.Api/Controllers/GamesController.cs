using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Api.Extensions;
using TicTacToe.Api.Infrastructure;
using TicTacToe.Application.Abstractions.Services;

namespace TicTacToe.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/games")]
public sealed class GamesController(IGamesService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Post()
    {
        var result = await service.CreateGameAsync();
        
        return result.Match(Results.Ok, CustomResults.Problem);
    }
    
    [HttpGet("{gameId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Get(int gameId)
    {
        var result = await service.GetGameByIdAsync(gameId);
        
        return result.Match(Results.Ok, CustomResults.Problem);
    }
}
