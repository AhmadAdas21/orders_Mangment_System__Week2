# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy project file and restore dependencies
COPY ["Week2_Task_2.csproj", "./"]
RUN dotnet restore "Week2_Task_2.csproj"

# Copy the rest of the project
COPY . .

# Publish the application
RUN dotnet publish "Week2_Task_2.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false


# Stage 2: Run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Week2_Task_2.dll"]