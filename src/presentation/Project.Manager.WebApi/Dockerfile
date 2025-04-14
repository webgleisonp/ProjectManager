FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["src/presentation/Project.Manager.WebApi/Project.Manager.WebApi.csproj", "src/presentation/Project.Manager.WebApi/"]
COPY ["src/presentation/Project.Manager.WebApi.Endpoints/Project.Manager.WebApi.Endpoints.csproj", "src/presentation/Project.Manager.WebApi.Endpoints/"]
COPY ["src/core/Project.Manager.Application/Project.Manager.Application.csproj", "src/core/Project.Manager.Application/"]
COPY ["src/core/Project.Manager.Domain/Project.Manager.Domain.csproj", "src/core/Project.Manager.Domain/"]
COPY ["src/infraestructure/Project.Manager.Infra.Data/Project.Manager.Infra.Data.csproj", "src/infraestructure/Project.Manager.Infra.Data/"]

RUN dotnet restore "src/presentation/Project.Manager.WebApi/Project.Manager.WebApi.csproj"

COPY . .

RUN dotnet build "src/presentation/Project.Manager.WebApi/Project.Manager.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/presentation/Project.Manager.WebApi/Project.Manager.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Project.Manager.WebApi.dll"] 