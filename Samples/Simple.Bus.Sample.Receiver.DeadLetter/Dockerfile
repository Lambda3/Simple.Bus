FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Simple.Bus.Receiver.DeadLetter/Simple.Bus.Receiver.DeadLetter.csproj", "Simple.Bus.Receiver.DeadLetter/"]
RUN dotnet restore "Simple.Bus.Receiver.DeadLetter/Simple.Bus.Receiver.DeadLetter.csproj"
COPY . .
WORKDIR "/src/Simple.Bus.Receiver.DeadLetter"
RUN dotnet build "Simple.Bus.Receiver.DeadLetter.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Simple.Bus.Receiver.DeadLetter.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Simple.Bus.Receiver.DeadLetter.dll"]