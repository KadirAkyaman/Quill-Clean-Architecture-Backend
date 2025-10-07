FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/Quill.Domain/Quill.Domain.csproj", "src/Quill.Domain/"]
COPY ["src/Quill.Application/Quill.Application.csproj", "src/Quill.Application/"]
COPY ["src/Quill.Infrastructure/Quill.Infrastructure.csproj", "src/Quill.Infrastructure/"]
COPY ["src/Quill.Presentation/Quill.Presentation.csproj", "src/Quill.Presentation/"]

RUN dotnet restore "src/Quill.Presentation/Quill.Presentation.csproj"

COPY src/ ./src/

RUN dotnet publish "src/Quill.Presentation/Quill.Presentation.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Quill.Presentation.dll"]