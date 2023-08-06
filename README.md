# CalculatorService

HTTP/REST ­based 'Calculator Service’ capable of some basic arithmetic operations, like add, subtract, square, etc. along with a history
service keeping track of requests sharing a common an identifier.

## Architecture & Components

The solution consists of two main components:

- CalculatorService.Server: The main component of the application, and the one actually implementing the 'calculator service’ HTTP/REST interface & business logic.
- CalculatorService.Client: A demonstration console/command­line client, capable of performing requests to the main HTTP service from CLI

## Requirements

- .NET 6 (LTS)
- Docker Desktop
- Visual Studio / Visual Studio Code

## Build

### CalculatorService.Server

From startup project folder: /src/Api/CalculatorService.Server:

> dotnet build -c Release

### CalculatorService.Client

From startup project folder: /src/Client/CalculatorService.Client:

> dotnet build -c Release

## Tests

From solution folder: /src

> dotnet test

It will run 3 test projects:

* Unit tests
  * CalculatorService.Server.UnitTests
  * CalculatorService.Client.UnitTests
* Integrationnit tests
  * CalculatorService.Server.IntegrationTests

## Deploy Server

### Deploy As Code

From solution folder: /src

> dotnet restore "./Api/CalculatorService.Server/CalculatorService.Server.csproj"
> dotnet build "./Api/CalculatorService.Server/CalculatorService.Server.csproj" -o \<build_folder\>
> dotnet publish "./Api/CalculatorService.Server/CalculatorService.Server.csproj" -c Release -o \<publish_folder\> /p:UseAppHost=false

To run the application from \<publish_folder\>:
> dotnet CalculatorService.Server.dll

### Deploy As Container

From server project folder: /src/Api/CalculatorService.Server:

> docker build -f Dockerfile .. -t \<registry\>:\<tag\>
> docker push \<registry\>:\<tag\>
> docker image pull \<registry\>:\<tag\>
> docker run \<registry\>:\<tag\>

Docker file needs to be built from Linux/WSL2 or use Dock Desktop instead.

## Server API reference

The API reference documententation will be available using:

* SwaggerUI: https://localhost:5001/swagger/index.html
* OpenAPI specification v3.0.1: https://localhost:5001/swagger/v1/swagger.json

## Client Usage

### Publish client

From solution folder: /src

> dotnet restore "./Client/CalculatorService.Client/CalculatorService.Client.csproj"
> dotnet build "./Client/CalculatorService.Client/CalculatorService.Client.csproj" -o \<build_folder\>
> dotnet publish "./Client/CalculatorService.Client/CalculatorService.Client.csproj" -c Release -o \<publish_folder\> /p:UseAppHost=false

### Client configuration

From \<publish_folder\> folder, edit "appSettings.json" file and set the value of "API:BaseAddress". (i.e. https://localhost:5001)

### Run client

To run the application from \<publish_folder\>:
> dotnet CalculatorService.Client.dll

The system will run a command prompt, enter "help" to see available commands, or enter a command name to get command specific help:

> calc\> help

Available commands: add, sub, mult, div, sqrt, journal