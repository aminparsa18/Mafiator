#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Mafiator/Mafiator.Api.csproj", "Mafiator/"]
COPY ["Mafiator.IocConfig/Mafiator.IocConfig.csproj", "Mafiator.IocConfig/"]
COPY ["Mafiator.Service/Mafiator.Service.csproj", "Mafiator.Service/"]
COPY ["MafiatorML.Model/MafiatorML.Model.csproj", "MafiatorML.Model/"]
COPY ["Mafiator.Repository/Mafiator.Repository.csproj", "Mafiator.Repository/"]
COPY ["Mafiator.Data/Mafiator.Data.csproj", "Mafiator.Data/"]
COPY ["Mafiator.Entities/Mafiator.Entities.csproj", "Mafiator.Entities/"]
COPY ["Mafiator.Common/Mafiator.Common.csproj", "Mafiator.Common/"]
RUN dotnet restore "Mafiator/Mafiator.Api.csproj"
COPY . .
WORKDIR "/src/Mafiator"
RUN dotnet build "Mafiator.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mafiator.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mafiator.Api.dll"]