# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/RunProcess/Workers/CR.RunProcess.Worker.Csharp/CR.RunProcess.Worker.Csharp.csproj", "Services/RunProcess/Workers/CR.RunProcess.Worker.Csharp/"]
COPY ["Library/Infrastructure/CR.RabbitMQ/CR.RabbitMQ.csproj", "Library/Infrastructure/CR.RabbitMQ/"]
COPY ["Services/Shared/CR.Constants/CR.Constants.csproj", "Services/Shared/CR.Constants/"]
COPY ["Library/CR.EntitiesBase/CR.EntitiesBase.csproj", "Library/CR.EntitiesBase/"]
COPY ["Services/RunProcess/Workers/CR.RunProcess.WorkerBase/CR.RunProcess.WorkerBase.csproj", "Services/RunProcess/Workers/CR.RunProcess.WorkerBase/"]
RUN dotnet restore "./Services/RunProcess/Workers/CR.RunProcess.Worker.Csharp/CR.RunProcess.Worker.Csharp.csproj"
COPY . .
WORKDIR "/src/Services/RunProcess/Workers/CR.RunProcess.Worker.Csharp"
RUN dotnet build "./CR.RunProcess.Worker.Csharp.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CR.RunProcess.Worker.Csharp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CR.RunProcess.Worker.Csharp.dll"]