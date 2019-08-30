FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["AvastApi/AvastApi.csproj", "AvastApi/"]
RUN dotnet restore "AvastApi/AvastApi.csproj"
COPY . .
WORKDIR "/src/AvastApi"
RUN dotnet build "AvastApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AvastApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AvastApi.dll"]