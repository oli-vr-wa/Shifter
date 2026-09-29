FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy all dependencies
COPY *.slnx ./
COPY Shifter.API/*.csproj ./Shifter.API/
COPY Shifter.Application/*.csproj ./Shifter.Application/
COPY Shifter.Core/*.csproj ./Shifter.Core/
COPY Shifter.Infrastructure/*.csproj ./Shifter.Infrastructure/

# Restore dependencies
RUN dotnet restore

COPY . ./

# Build and publish targeting the main API project
WORKDIR /app/Shifter.API
RUN dotnet publish -c Release -o /app/out

# Setup the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 5170
ENTRYPOINT ["dotnet", "Shifter.API.dll"]