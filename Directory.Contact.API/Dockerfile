FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Directory.Contact.API/Directory.Contact.API.csproj", "Directory.Contact.API/"]
RUN dotnet restore "Directory.Contact.API/Directory.Contact.API.csproj"
COPY . .
WORKDIR "/src/Directory.Contact.API"
RUN dotnet build "Directory.Contact.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Directory.Contact.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Directory.Contact.API.dll"]
