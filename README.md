# ModerApiTest

Web API sample with C#9 and ASP.NET Core 5.0.

## Dependencies
* .NET Core SDK 5.0 (available at https://dotnet.microsoft.com/download)
* Docker
* Mongodb
* Nginx

## Build and run unit tests locally
	To pull the required docker images, build and run, simply type the following command:
```console
	run
```

## Content

* folder ModerApiTest : the project implementing the web API
* folder ModerApiTest.DAL : the Data Access Layer, responsible of the access to the mongodb database
* folder Nginx : configurtion of nginx
* folder Postman : a postman collection to test the API

## Containers

In that application 3 docker containers are running:
* a nginx front end used as reverse proxy, serving requests to the server.
* a server implementing web APIs, listening the requests coming from the frontend and accessing the mongodb database.
* a mongodb database treating requests coming from the server by maintaining 2 collections: users and articles.
