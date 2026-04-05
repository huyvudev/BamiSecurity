#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Core/CR.Core.API/CR.Core.API.csproj", "Services/Core/CR.Core.API/"]
COPY ["Library/Infrastructure/CR.LLM/CR.LLM.csproj", "Library/Infrastructure/CR.LLM/"]
COPY ["Library/CR.ApplicationBase/CR.ApplicationBase.csproj", "Library/CR.ApplicationBase/"]
COPY ["Library/Application/CR.Localization/CR.Localization.csproj", "Library/Application/CR.Localization/"]
COPY ["Library/CR.DtoBase/CR.DtoBase.csproj", "Library/CR.DtoBase/"]
COPY ["Library/Infrastructure/CR.ConstantBase/CR.ConstantBase.csproj", "Library/Infrastructure/CR.ConstantBase/"]
COPY ["Library/CR.InfrastructureBase/CR.InfrastructureBase.csproj", "Library/CR.InfrastructureBase/"]
COPY ["Services/Shared/CR.Constants/CR.Constants.csproj", "Services/Shared/CR.Constants/"]
COPY ["Library/CR.EntitiesBase/CR.EntitiesBase.csproj", "Library/CR.EntitiesBase/"]
COPY ["Library/CR.Utils/CR.Utils.csproj", "Library/CR.Utils/"]
COPY ["Library/Infrastructure/CR.S3Bucket/CR.S3Bucket.csproj", "Library/Infrastructure/CR.S3Bucket/"]
COPY ["Library/CR.IdentityServerBase/CR.IdentityServerBase.csproj", "Library/CR.IdentityServerBase/"]
COPY ["Services/Core/CR.Core.Infrastructure/CR.Core.Infrastructure.csproj", "Services/Core/CR.Core.Infrastructure/"]
COPY ["Services/Core/CR.Core.Domain/CR.Core.Domain.csproj", "Services/Core/CR.Core.Domain/"]
COPY ["Library/CR.WebAPIBase/CR.WebAPIBase.csproj", "Library/CR.WebAPIBase/"]
COPY ["Services/Shared/CR.Common/CR.Common.csproj", "Services/Shared/CR.Common/"]
COPY ["Library/Infrastructure/CR.ConvertFile/CR.ConvertFile.csproj", "Library/Infrastructure/CR.ConvertFile/"]
COPY ["Library/Infrastructure/CR.RabbitMQ/CR.RabbitMQ.csproj", "Library/Infrastructure/CR.RabbitMQ/"]
COPY ["Tools/CR.Project.ServiceDefaults/CR.Project.ServiceDefaults.csproj", "Tools/CR.Project.ServiceDefaults/"]
COPY ["Services/Shared/CR.UserRolePermission/CR.UserRolePermission.csproj", "Services/Shared/CR.UserRolePermission/"]
COPY ["Services/Core/CR.Core.ApplicationServices/CR.Core.ApplicationServices.csproj", "Services/Core/CR.Core.ApplicationServices/"]
COPY ["Library/Infrastructure/OpenXMLLibrary/OpenXMLLibrary.csproj", "Library/Infrastructure/OpenXMLLibrary/"]
COPY ["Services/Core/CR.Core.Dtos/CR.Core.Dtos.csproj", "Services/Core/CR.Core.Dtos/"]
RUN dotnet restore "./Services/Core/CR.Core.API/CR.Core.API.csproj"
COPY . .
WORKDIR "/src/Services/Core/CR.Core.API"
RUN dotnet build "./CR.Core.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CR.Core.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CR.Core.API.dll"]