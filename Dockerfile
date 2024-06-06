FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

ARG CERT_PASSWORD

COPY *.csproj ./
RUN dotnet restore

RUN dotnet new tool-manifest
RUN dotnet tool install --local dotnet-ef


RUN mkdir -p /app/certificates
RUN dotnet dev-certs https -ep /app/certificates/aspnetapp.pfx -p $CERT_PASSWORD
RUN dotnet dev-certs https --trust
RUN ls -la /app/certificates/

COPY . ./
RUN dotnet publish -c Release -o out


RUN dotnet ef migrations add InitialCreate
RUN dotnet ef database update

RUN cp sqlite.db out/


FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=build-env /app/certificates/aspnetapp.pfx /app/certificates/

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "backend lembrol.dll"]
