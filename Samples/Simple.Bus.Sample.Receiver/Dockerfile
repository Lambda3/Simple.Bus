FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Simple.Bus.Sample.Receiver/Simple.Bus.Sample.Receiver.csproj", "Simple.Bus.Sample.Receiver/"]
RUN dotnet restore "Simple.Bus.Sample.Receiver/Simple.Bus.Sample.Receiver.csproj"
COPY . .
WORKDIR "/src/Simple.Bus.Sample.Receiver"
RUN dotnet build "Simple.Bus.Sample.Receiver.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Simple.Bus.Sample.Receiver.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Simple.Bus.Sample.Receiver.dll"]