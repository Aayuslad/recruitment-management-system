FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ./Server.sln ./
COPY ./Server.API/Server.API.csproj ./Server.API/
COPY ./Server.Application/Server.Application.csproj ./Server.Application/
COPY ./Server.Infrastructure/Server.Infrastructure.csproj ./Server.Infrastructure/
COPY ./Server.Core/Server.Core.csproj ./Server.Core/
COPY ./Server.Domain/Server.Domain.csproj ./Server.Domain/

RUN dotnet restore

COPY . .

RUN dotnet publish ./Server.API/Server.API.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_ENVIRONMENT="Production"

EXPOSE 5126
ENTRYPOINT ["dotnet", "Server.API.dll"]