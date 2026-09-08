FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all files into /src
COPY . .

# Move into the subfolder where ApiSampleForSDLC.csproj is located
WORKDIR /src/ApiSampleForSDLC

RUN dotnet restore "ApiSampleForSDLC.csproj"
RUN dotnet publish "ApiSampleForSDLC.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "ApiSampleForSDLC.dll"]
