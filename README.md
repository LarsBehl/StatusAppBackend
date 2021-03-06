# StatusAppBackend
This is the backend for an upcoming `.NET MAUI` application for visualizing the current state (UP, DOWN, response time, etc.) of a web application or service.

## About
This service was created as part of a university project during my masters degree in computer science. It serves as the needed backend for a `.NET MAUI` application, visualizing the provided information. I don't really know if this repository will be updated frequently after finishing the university project. However I have personal interest in adding new features to the backend, e.g. adding notification via e-mail when a service is unreachable.

## Getting started
To be able to run the application the following components need to be installed:
* .NET 6 SDK
* EF-Core command line tools
* Docker or PostgreSQL Server

### Setup
After installing the needed components and cloning this repository, close to everything is ready to go. To run the app please follow these steps:
1. Add `appsettings.Development.json` to the root of this repository
2. Copy the content from the existing `appsettings.json` and set the `LogLevel` to `Debug`
3. Create a postgres database or docker container. This command will generate a usable container
```shell
docker run --name postgres -e POSTGRES_PASSWORD=root -e POSTGRES_DB=statusapp -d --network=host postgres
```
4. Add the connection string for the database to the created `appsettings.Development.json`
5. In the `appsettings.Development.json` create an JSON-object named `AppSettings`
6. Add a property `Secret` which will be used as the secret for created JWTs
7. Add a property `IsDevelopment` and set it to true
8. Run the following command to update the database
```shell
dotnet ef database update
```
9. Run the application via
```shell
dotnet run
```

After following these steps everything _should_ work just fine. However if you have any issues, feel free to reach out.

On the initial launch of the application a token should be printed on the console. This token can be used to create the first user/admin of the application. This process is mandatory to be able to configure the service to query other web applications or services. When opening up the browser and navigating to https://localhost:5001/swagger you will be presented with the `OpenAPI` specification of this service. In there you will find all resources exposed by this service and the needed routes to configure querying for other services.

## Contribution
As this is a small project, contributing is fairly simple, but I still want to follow certain steps to keep everything transparent:
1. Open an issue describing the bug or feature that should be added.
2. If you think you can fix the issue or implement the feature by yourself, please take a look into the existing files and get familiar with the style used in this project.
3. Create a branch from the `master` branch where you will work on the issue following the naming convention: {bug | feature}/{issue-id}_{issue-title}
4. When you are finished with your work or want somebody else to take a look at your code, open a pull request to merge your branch into the master branch.
5. The code will be reviewed by the maintainers. If everything is alright, the pull request will be merged and your development branch deleted.

