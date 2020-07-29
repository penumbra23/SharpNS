FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS builder
WORKDIR /app

COPY SharpNS/*.csproj ./
RUN dotnet restore

COPY SharpNS/* ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=builder /app/out .

EXPOSE 80

ENTRYPOINT ["dotnet", "SharpNS.dll"]