using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TicTacToe.Application.Abstractions.Services;
using TicTacToe.Application.Services;

namespace TicTacToe.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSerilog((services, configuration) => configuration
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext());

        RegisterServices(builder);
    }

    private static void RegisterServices(IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IMovesService, MovesService>();
        builder.Services.AddScoped<IGamesService, GamesService>();
    }
}
