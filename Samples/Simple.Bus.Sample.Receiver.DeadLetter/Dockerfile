FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Simple.Bus.Core/Simple.Bus.Core.csproj", "Simple.Bus.Core/"]
COPY ["Samples/Simple.Bus.Sample.Receiver.DeadLetter/Simple.Bus.Sample.Receiver.DeadLetter.csproj", "Samples/Simple.Bus.Sample.Receiver.DeadLetter/"]
RUN dotnet restore "Samples/Simple.Bus.Sample.Receiver.DeadLetter/Simple.Bus.Sample.Receiver.DeadLetter.csproj"
COPY . .
RUN dotnet publish "/src/Samples/Simple.Bus.Sample.Receiver.DeadLetter/Simple.Bus.Sample.Receiver.DeadLetter.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
ENTRYPOINT ["/app/Simple.Bus.Sample.Receiver.DeadLetter"]
ENV DOTNET_ENVIRONMENT=Development
COPY --from=build /app/publish .