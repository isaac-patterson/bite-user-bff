#FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
FROM mcr.microsoft.com/dotnet/sdk:5.0-focal-amd64 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal
WORKDIR /app
COPY --from=build-env /app/out .

ENV ASPNETCORE_URLS http://*:443
ENTRYPOINT ["dotnet",  "user-bff.dll"]