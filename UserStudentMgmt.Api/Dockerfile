FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UserStudentMgmt.Api/UserStudentMgmt.Api.csproj", "UserStudentMgmt.Api/"]
COPY ["UserStudentMgmt.Application/UserStudentMgmt.Application.csproj", "UserStudentMgmt.Application/"]
COPY ["UserStudentMgmt.Domain/UserStudentMgmt.Domain.csproj", "UserStudentMgmt.Domain/"]
COPY ["UserStudentMgmt.Infrastructure/UserStudentMgmt.Infrastructure.csproj", "UserStudentMgmt.Infrastructure/"]
RUN dotnet restore "UserStudentMgmt.Api/UserStudentMgmt.Api.csproj"
COPY . .
WORKDIR "/src/UserStudentMgmt.Api"
RUN dotnet build "./UserStudentMgmt.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UserStudentMgmt.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserStudentMgmt.Api.dll"]
