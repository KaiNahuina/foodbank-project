#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
ENV HOST '0.0.0.0'
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Foodbank Project.csproj", "."]
RUN dotnet restore "./Foodbank Project.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Foodbank Project.csproj" -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "Foodbank Project.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Foodbank Project.dll"]