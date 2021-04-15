FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Simple.Bus.Core/Simple.Bus.Core.csproj", "Simple.Bus.Core/"]
COPY ["Samples/Simple.Bus.Sample.Receiver/Simple.Bus.Sample.Receiver.csproj", "Samples/Simple.Bus.Sample.Receiver/"]
RUN dotnet restore "Samples/Simple.Bus.Sample.Receiver/Simple.Bus.Sample.Receiver.csproj"
COPY . .
RUN dotnet publish "/src/Samples/Simple.Bus.Sample.Receiver/Simple.Bus.Sample.Receiver.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
ENTRYPOINT ["/app/Simple.Bus.Sample.Receiver"]
ENV DOTNET_ENVIRONMENT=Development
ENV Bus__MessageContract__RabbitMQ__HostName=host.docker.internal
COPY --from=build /app/publish .