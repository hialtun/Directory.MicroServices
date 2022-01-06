FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Directory.Report.API/Directory.Report.API.csproj", "Directory.Report.API/"]
RUN dotnet restore "Directory.Report.API/Directory.Report.API.csproj"
COPY . .
WORKDIR "/src/Directory.Report.API"
RUN dotnet build "Directory.Report.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Directory.Report.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Directory.Report.API.dll"]
