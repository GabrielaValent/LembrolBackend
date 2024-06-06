FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

RUN dotnet new tool-manifest
RUN dotnet tool install --local dotnet-ef

COPY . ./
RUN dotnet publish -c Release -o out && ls out

RUN dotnet ef migrations add InitialCreate
RUN dotnet ef database update
RUN cp sqlite.db out/

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "backend lembrol.dll"]