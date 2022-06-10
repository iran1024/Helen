#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FileStorage/HelenServer.FileStorage.Web/HelenServer.FileStorage.Web.csproj", "FileStorage/HelenServer.FileStorage.Web/"]
COPY ["HelenServer.Grpc.Contracts/HelenServer.Grpc.Contracts.csproj", "HelenServer.Grpc.Contracts/"]
COPY ["HelenServer.Core/HelenServer.Core.csproj", "HelenServer.Core/"]
COPY ["FileStorage/HelenServer.FileStorage.Controllers/HelenServer.FileStorage.Controllers.csproj", "FileStorage/HelenServer.FileStorage.Controllers/"]
COPY ["FileStorage/HelenServer.FileStorage.Contracts/HelenServer.FileStorage.Contracts.csproj", "FileStorage/HelenServer.FileStorage.Contracts/"]
COPY ["HelenServer.Dapr/HelenServer.Dapr.csproj", "HelenServer.Dapr/"]
COPY ["FileStorage/HelenServer.FileStorage.Dal/HelenServer.FileStorage.Dal.csproj", "FileStorage/HelenServer.FileStorage.Dal/"]
COPY ["HelenServer.Data.SqlServer/HelenServer.Data.SqlServer.csproj", "HelenServer.Data.SqlServer/"]
COPY ["HelenServer.Common/HelenServer.Common.csproj", "HelenServer.Common/"]
COPY ["FileStorage/HelenServer.FileStorage.Services/HelenServer.FileStorage.Services.csproj", "FileStorage/HelenServer.FileStorage.Services/"]
COPY ["FileStorage/HelenServer.FileStorage.FastDFS/HelenServer.FileStorage.FastDFS.csproj", "FileStorage/HelenServer.FileStorage.FastDFS/"]
RUN dotnet restore "FileStorage/HelenServer.FileStorage.Web/HelenServer.FileStorage.Web.csproj"
COPY . .
WORKDIR "/src/FileStorage/HelenServer.FileStorage.Web"
RUN dotnet build "HelenServer.FileStorage.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HelenServer.FileStorage.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HelenServer.FileStorage.Web.dll"]