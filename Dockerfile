FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY [".", "."]

RUN cd Void.Server/
RUN dotnet restore "DarkSun.Engine.Runner/DarkSun.Engine.Runner.csproj"
COPY . .
WORKDIR "/src/DarkSun.Engine.Runner"
RUN dotnet build "DarkSun.Engine.Runner.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DarkSun.Engine.Runner.csproj" -c Release -r linux-x64 -p:PublishReadyToRun=true -p:PublishSingleFile=true --self-contained true -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./DarkSun.Engine.Runner"]
