FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AR.Bot.Web/AR.Bot.Web.csproj", "AR.Bot.Web/"]
RUN dotnet restore "AR.Bot.Web/AR.Bot.Web.csproj"
COPY . .
WORKDIR "/src/AR.Bot.Web"
RUN dotnet build "AR.Bot.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AR.Bot.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AR.Bot.Web.dll"]