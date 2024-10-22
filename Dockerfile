# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# COPY ["ContactManager/ContactManager.csproj", "ContactManager/"]
COPY . .
# WORKDIR "/src/ContactManager"
RUN dotnet restore "ContactManager.csproj"
RUN dotnet build "ContactManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContactManager.csproj" -c Release -o /app/publish

# Use the runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContactManager.dll"]
