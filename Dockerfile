FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /Projects/Mafiator

# copy csproj and restore as distinct layers
COPY GameService/*.csproj ./

#RUN dotnet restore

# copy and publish app and libraries
COPY ./GameService .
RUN dotnet publish -c release -o /app 

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "GameService.dll"]