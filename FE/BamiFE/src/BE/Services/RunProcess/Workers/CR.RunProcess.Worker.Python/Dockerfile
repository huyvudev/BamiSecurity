#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/RunProcess/Workers/CR.RunProcess.Worker.Python/CR.RunProcess.Worker.Python.csproj", "Services/RunProcess/Workers/CR.RunProcess.Worker.Python/"]
COPY ["Services/RunProcess/Workers/CR.RunProcess.WorkerBase/CR.RunProcess.WorkerBase.csproj", "Services/RunProcess/Workers/CR.RunProcess.WorkerBase/"]
RUN dotnet restore "./Services/RunProcess/Workers/CR.RunProcess.Worker.Python/CR.RunProcess.Worker.Python.csproj"
COPY . .
WORKDIR "/src/Services/RunProcess/Workers/CR.RunProcess.Worker.Python"
RUN dotnet build "./CR.RunProcess.Worker.Python.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CR.RunProcess.Worker.Python.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CR.RunProcess.Worker.Python.dll"]