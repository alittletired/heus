ARG NET_IMAGE=5.0-buster-slim
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Heus.Web/Heus.Web.csproj", "Heus.Web/"]
RUN dotnet restore "Heus.Web/Heus.Web.csproj"
COPY . .
WORKDIR "/src/Heus.Web"
RUN dotnet build "Heus.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Heus.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Heus.Web.dll"]
