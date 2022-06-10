#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BugEngine/HelenServer.BugEngine.Web/HelenServer.BugEngine.Web.csproj", "BugEngine/HelenServer.BugEngine.Web/"]
COPY ["BugEngine/HelenServer.BugEngine.Controllers/HelenServer.BugEngine.Controllers.csproj", "BugEngine/HelenServer.BugEngine.Controllers/"]
COPY ["BugEngine/HelenServer.BugEngine.Contracts/HelenServer.BugEngine.Contracts.csproj", "BugEngine/HelenServer.BugEngine.Contracts/"]
COPY ["HelenServer.Core/HelenServer.Core.csproj", "HelenServer.Core/"]
COPY ["HelenServer.Grpc.Contracts/HelenServer.Grpc.Contracts.csproj", "HelenServer.Grpc.Contracts/"]
COPY ["BugEngine/HelenServer.BugEngine.Services/HelenServer.BugEngine.Services.csproj", "BugEngine/HelenServer.BugEngine.Services/"]
COPY ["BugEngine/HelenServer.BugEngine.Dal/HelenServer.BugEngine.Dal.csproj", "BugEngine/HelenServer.BugEngine.Dal/"]
COPY ["HelenServer.Data.SqlServer/HelenServer.Data.SqlServer.csproj", "HelenServer.Data.SqlServer/"]
COPY ["HelenServer.Dapr/HelenServer.Dapr.csproj", "HelenServer.Dapr/"]
RUN dotnet restore "BugEngine/HelenServer.BugEngine.Web/HelenServer.BugEngine.Web.csproj"
COPY . .
WORKDIR "/src/BugEngine/HelenServer.BugEngine.Web"
RUN dotnet build "HelenServer.BugEngine.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HelenServer.BugEngine.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HelenServer.BugEngine.Web.dll"]