FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["lab_1/lab_1.csproj", "lab_1/"]
RUN dotnet restore "lab_1/lab_1.csproj"
COPY . .
WORKDIR "/src/lab_1"
RUN dotnet build "lab_1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "lab_1.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "lab_1.dll"]
