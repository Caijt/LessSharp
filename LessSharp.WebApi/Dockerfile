#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["LessSharp.WebApi/LessSharp.WebApi.csproj", "LessSharp.WebApi/"]
COPY ["LessSharp.Option/LessSharp.Option.csproj", "LessSharp.Option/"]
COPY ["LessSharp.Common/LessSharp.Common.csproj", "LessSharp.Common/"]
COPY ["LessSharp.Service/LessSharp.Service.csproj", "LessSharp.Service/"]
COPY ["LessSharp.Entity/LessSharp.Entity.csproj", "LessSharp.Entity/"]
COPY ["LessSharp.ApiService/LessSharp.ApiService.csproj", "LessSharp.ApiService/"]
COPY ["LessSharp.Dto/LessSharp.Dto.csproj", "LessSharp.Dto/"]
COPY ["LessSharp.MapperConfiguration/LessSharp.MapperConfiguration.csproj", "LessSharp.MapperConfiguration/"]
COPY ["LessSharp.Data/LessSharp.Data.csproj", "LessSharp.Data/"]
COPY ["LessSharp.EntityConfiguration/LessSharp.EntityConfiguration.csproj", "LessSharp.EntityConfiguration/"]
RUN dotnet restore "LessSharp.WebApi/LessSharp.WebApi.csproj"
COPY . .
WORKDIR "/src/LessSharp.WebApi"
RUN dotnet build "LessSharp.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LessSharp.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LessSharp.WebApi.dll"]