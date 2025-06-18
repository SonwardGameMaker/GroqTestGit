# Встановлюємо базовий Linux SDK для білду
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо проект і відновлюємо залежності
COPY ["GroqTest/GroqTest.csproj", "GroqTest/"]
RUN dotnet restore "GroqTest/GroqTest.csproj"

# Копіюємо все інше і будуємо
COPY . .
WORKDIR "/src/GroqTest"
RUN dotnet publish "GroqTest.csproj" -c Release -o /app/publish

# Встановлюємо runtime Linux і копіюємо білд
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GroqTest.dll"]
