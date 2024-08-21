The goal of this document is to highlight some features of this solution.

## Pre-requisites
- .NET 8
- Docker
- IDE such as VsCode, Visual Studio, Rider 


## How to run it?
The only way to run it at the moment it through docker compose. There are couple of docker compose files included in the src folder.

- [docker-compose.yml](./src/docker-compose.yml) - This file is for running the app and its supporting services in Release mode. 
- [docker-compose.debug.yml](./src/docker-compose.debug.yml.yml) - This file is for running the app and its supporting services in Debug mode. You can also attached debugger to the app if you run it using this file.

Steps to run the application 

- Open terminal and change directory into the [root folder](.)
- Run one of the following commands depending on what mode you want to run the app in.
`docker compose -f src/docker-compose.yml up`

`docker compose -f src/docker-compose.debug.yml up`
- Open your browser and navigate to the following link to open the swagger doc.
  `http://localhost:3000/swagger`


## Tests
All tests can be found in the tests folder and are grouped under one test project called [AppointmentSystems.Tests.csproj](./tests/AppointmentSystem.Tests/AppointmentSystem.Tests.csproj). You don't need to have the docker compose up and running to run the tests. Tests (integration tests) manage their own container creation using a package called [TestContainers](https://testcontainers.com/).

There are two types of tests.

- Unit tests - These only check individual classes of the application. You can run them via the follwoing command in the [project root directory](./).For these tests you don't need docker up and running.
  `dotnet test AppointmentSystem.sln --filter "Type=Unit" `

- Integration Tests - These tests check the logic of fetching data the database. To run them you need docker up and running. And use can use the following command in the project [root directory](./)
  ` dotnet test AppointmentSystem.sln --filter "Type=Integration"`

    Note : Tests use the same migration scripts as the application database to create and pouplate the data and all other necessery resources in the database. More on that below.


## Migrations Projects
To initalize and load the data in the database and the test database we use the migration projects. 
- [AppointmentSystsem.Migration](./src/AppointmentSystem.Migration/) - This project contains all the sql scripts including the [init.sql](./src/AppointmentSystem.Migration/Scripts/00001-init.sql) script that creates and pouplates all the necessery resoruces in the database. It also contains new scripts for creating [indexes](./src//AppointmentSystem.Migration//Scripts/00002-CreateIndex.sql) and a [view](./src/AppointmentSystem.Migration/Scripts//00003-CreateView.sql).
- [AppointmentSystem.MigrationRunner](./src/AppointmentSystem.MigrationRunner/) - The project runs the migration on the application database (not on the test database).


## [Requests Folder](./requests/)
This folder contains a .http file called [AppointmentSystemApi](./requests/AppointmentSystemApi.http). This file esentially contains all the tests cases found in the index.js file in an http request format. This was mainly used to manually tests the api during developement.