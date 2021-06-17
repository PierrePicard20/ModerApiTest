# ModerApiTest

A Web API sample application in C#9 and ASP.NET Core 5.0.

## Dependencies
* .NET Core SDK 5.0
* Docker
* Mongodb
* Nginx

Only Docker is actually required to be installed to run the application as other dependencies will be fetched as docker images.

## To build and run locally
	To pull the required docker images, build and run, simply type the following command:
```console
	run
```

## Content

* folder ModerApiTest : the project implementing the web API
* folder ModerApiTest.DAL : the Data Access Layer project, responsible of the access to the mongodb database
* folder Nginx : configuration of nginx
* folder Postman : a postman collection to test each endpoint of the API

## Containers

In that application 3 docker containers are running:
* a nginx front end used as reverse proxy, serving requests to the server.
* a server implementing the web APIs, listening the requests coming from the frontend and accessing the mongodb database.
* a mongodb database treating requests coming from the server by maintaining 2 collections: users and articles.
