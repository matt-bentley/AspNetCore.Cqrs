FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim AS base

WORKDIR /app

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim AS build

COPY ["src/", "/src/"]

WORKDIR /src/AspNetCore.Cqrs.Migrations
RUN dotnet restore "AspNetCore.Cqrs.Migrations.csproj" && \
    dotnet publish "AspNetCore.Cqrs.Migrations.csproj" --no-restore -c Release -o /app/publish

FROM base AS final
COPY --from=build /app/publish .
USER 1000
ENTRYPOINT ["dotnet", "AspNetCore.Cqrs.Migrations.dll"]