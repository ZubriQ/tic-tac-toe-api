namespace TicTacToe.Api.Extensions;

internal static class WebApplicationExtensions
{
    public static void MapOpenApiWithScalar(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return;
        }

        app.MapOpenApi();
    }
}
