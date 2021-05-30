#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# build an image based on asp.net core 5.0
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

#build another image based on the sdk for compilation purpose of the project
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ModerApiTest/ModerApiTest.csproj", "ModerApiTest/"]
RUN dotnet restore "ModerApiTest/ModerApiTest.csproj"
COPY . .
WORKDIR "/src/ModerApiTest"
RUN dotnet build "ModerApiTest.csproj" -c Release -o /app/build

# on the same image execute dotnet publish to put together all the assests
FROM build AS publish
RUN dotnet publish "ModerApiTest.csproj" -c Release -o /app/publish

# add the previously published application in the asp.net core image 
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ModerApiTest.dll"]