using TicTacToe.Application.Options;

namespace TicTacToe.Api;

internal static class DependencyInjection
{
    public static void AddWebServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();
        
        builder.Services.Configure<GameOptions>(builder.Configuration.GetSection(nameof(GameOptions)));

        builder.Services
            .AddApiVersioning()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
    }
}
