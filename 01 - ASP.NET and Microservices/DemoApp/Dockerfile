FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine as build

WORKDIR /app
COPY ./DemoApp ./DemoApp
COPY ./DemoClassLibrary ./DemoClassLibrary
COPY ./DemoApp.sln .
RUN dotnet publish ./DemoApp.sln -c Release -o ./artifacts

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine
WORKDIR /app
COPY --from=build /app/artifacts .
ENTRYPOINT [ "dotnet", "/app/DemoApp.dll" ]