using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Api.Extensions;
using TicTacToe.Api.Filters;
using TicTacToe.Api.Infrastructure;
using TicTacToe.Application.Abstractions.Services;
using TicTacToe.Application.Dtos;
using TicTacToe.Contracts.Moves;

namespace TicTacToe.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/games/{gameId:int}/moves")]
public sealed class MovesController(IMovesService service) : ControllerBase
{
    [HttpPost]
    [Idempotent]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Post(CreateMoveRequest request, int gameId)
    {
        var result = await service.CreateByGameIdAsync(
            new CreateMoveCommand(gameId, request.Row, request.Column));
        
        return result.Match(Results.Ok, CustomResults.Problem);
    }
}
