FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ReportService.sln ./
COPY ReportService.Api/ReportService.Api.csproj ReportService.Api/
COPY ReportService.BusinessLogic/ReportService.BusinessLogic.csproj ReportService.BusinessLogic/
COPY ReportService.DataAccess/ReportService.DataAccess.csproj ReportService.DataAccess/
COPY ReportService.Integrations/ReportService.Integrations.csproj ReportService.Integrations/
RUN dotnet restore
COPY . .
WORKDIR /src/ReportService.Api
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReportService.Api.dll"]
