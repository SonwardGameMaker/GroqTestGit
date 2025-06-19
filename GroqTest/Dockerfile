FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GroqTest/GroqTest.csproj", "GroqTest/"]
RUN dotnet restore "GroqTest/GroqTest.csproj"
COPY . .
WORKDIR "/src/GroqTest"
RUN dotnet publish "GroqTest.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# 🟢 Вказуємо, що додаток слухає порт
EXPOSE 8080

ENTRYPOINT ["dotnet", "GroqTest.dll"]
