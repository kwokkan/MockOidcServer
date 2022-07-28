#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0.7-alpine3.16 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0.302-alpine3.16 AS build
WORKDIR /src
COPY ["src/MockOidcServer.Web/MockOidcServer.Web.csproj", "src/MockOidcServer.Web/"]
RUN dotnet restore "src/MockOidcServer.Web/MockOidcServer.Web.csproj"
COPY . .
WORKDIR "/src/src/MockOidcServer.Web"
RUN dotnet publish "MockOidcServer.Web.csproj" -c Release --no-restore -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MockOidcServer.Web.dll"]
