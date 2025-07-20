using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using TicTacToe.Api;
using TicTacToe.Application;
using TicTacToe.Infrastructure;
using TicTacToe.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.AddApplication();
builder.AddInfrastructure();
builder.AddWebServices();

var app = builder.Build();

await app.InitializeDatabaseAsync();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseSerilogRequestLogging();

app.MapControllers();

await app.RunAsync();

// REMARK: Требуется для функциональных и интеграционных тестов.
namespace TicTacToe.Api
{
    public partial class Program;
}
