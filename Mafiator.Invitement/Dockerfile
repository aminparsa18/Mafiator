FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /Projects/Mafiator/Mafiator.Invitement
EXPOSE 80
EXPOSE 443
# copy csproj and restore as distinct layers
COPY /*.csproj ./

RUN dotnet restore

# copy and publish app and libraries
COPY . .
RUN dotnet publish -c release -o /app 

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Mafiator.Invitement.dll"]