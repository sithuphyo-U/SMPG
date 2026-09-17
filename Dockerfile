FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["DIS.web/DIS.Web.csproj", "DIS.web/"]
COPY ["DIS.Application/DIS.Application.csproj", "DIS.Application/"]
COPY ["DIS.DataAccess/DIS.DataAccess.csproj", "DIS.DataAccess/"]
COPY ["DIS.Infrastruture/DIS.Infrastruture.csproj", "DIS.Infrastruture/"]

RUN dotnet restore "DIS.web/DIS.Web.csproj"

COPY . .

WORKDIR "/src/DIS.web"

RUN dotnet publish "DIS.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000

EXPOSE 10000

ENTRYPOINT ["dotnet", "DIS.Web.dll"]