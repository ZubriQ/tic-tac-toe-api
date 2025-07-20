FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TicTacToe.Api/TicTacToe.Api.csproj", "src/TicTacToe.Api/"]
COPY ["src/TicTacToe.Application/TicTacToe.Application.csproj", "src/TicTacToe.Application/"]
COPY ["src/TicTacToe.Domain/TicTacToe.Domain.csproj", "src/TicTacToe.Domain/"]
COPY ["src/TicTacToe.Contracts/TicTacToe.Contracts.csproj", "src/TicTacToe.Contracts/"]
COPY ["src/TicTacToe.Infrastructure/TicTacToe.Infrastructure.csproj", "src/TicTacToe.Infrastructure/"]
RUN dotnet restore "src/TicTacToe.Api/TicTacToe.Api.csproj"
COPY . .
WORKDIR "/src/src/TicTacToe.Api"
RUN dotnet build "./TicTacToe.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TicTacToe.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TicTacToe.Api.dll"]
