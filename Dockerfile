FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY [".", "."]

RUN cd DarkStar.Engine.Runner/
RUN dotnet restore "DarkStar.Engine.Runner/DarkStar.Engine.Runner.csproj"
COPY . .
WORKDIR "/src/DarkStar.Engine.Runner"
RUN dotnet build "DarkStar.Engine.Runner.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DarkStar.Engine.Runner.csproj" -c Release -r linux-x64 -p:PublishReadyToRun=true --self-contained true -o /app/publish

FROM base AS final
WORKDIR /app
ENV DOCKER_CONTAINER=true
COPY --from=publish /app/publish .
ENTRYPOINT ["./DarkStar.Engine.Runner"]
