using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using TicTacToe.Application.Abstractions.Clock;
using TicTacToe.Application.Abstractions.Data;
using TicTacToe.Application.Abstractions.Random;
using TicTacToe.Application.Abstractions.TicTacToe;
using TicTacToe.Infrastructure.Clock;
using TicTacToe.Infrastructure.Data;
using TicTacToe.Infrastructure.Random;
using TicTacToe.Infrastructure.TicTacToe;

namespace TicTacToe.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("Database");

        builder.Services.AddDbContext<ApplicationDbContext>(x => 
            x.UseNpgsql(connectionString, options => options.CommandTimeout(30))
                .UseSnakeCaseNamingConvention());

        builder.Services.AddScoped<IApplicationDbContext>(provider => 
            provider.GetRequiredService<ApplicationDbContext>());
        
        builder.Services.AddScoped<ApplicationDbContextInitializer>();
        
        builder.Services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();
        builder.Services.TryAddSingleton<IRandomProvider, RandomProvider>();
        
        builder.Services.AddScoped<ITicTacToeGameEngine, TicTacToeGameEngine>();
        
        builder.Services.AddDistributedMemoryCache();
        
        builder.Services.AddHealthChecks()
            .AddNpgSql(connectionString!);
    }
}
