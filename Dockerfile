#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Customer.Notify.Microservice.API/Customer.Notify.Microservice.API.csproj", "Customer.Notify.Microservice.API/"]
COPY ["Customer.Notify.Microservice.APP/Customer.Notify.Microservice.APP.csproj", "Customer.Notify.Microservice.APP/"]
COPY ["Customer.Notify.Microservice/Customer.Notify.Microservice.Domain.csproj", "Customer.Notify.Microservice/"]
COPY ["Customer.Notify.Microservice.Infrastructure/Customer.Notify.Microservice.Infrastructure.csproj", "Customer.Notify.Microservice.Infrastructure/"]
RUN dotnet restore "Customer.Notify.Microservice.API/Customer.Notify.Microservice.API.csproj"
COPY . .
WORKDIR "/src/Customer.Notify.Microservice.API"
RUN dotnet build "Customer.Notify.Microservice.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Customer.Notify.Microservice.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Customer.Notify.Microservice.API.dll"]