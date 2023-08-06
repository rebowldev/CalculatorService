# CalculatorService

HTTP/REST ­based 'Calculator Service’ capable of some basic arithmetic operations along with a history service keeping track of requests sharing a common  identifier.

The supported arithmetic operations are:

* Addition (add): adds two or mor decimal numbers and returns their sum.
* Substraction (sub): substracts two decimal numbers and returns the difference.
* Multiplication (mult): multiplies two or more decimal numbers and returns their product.
* Division (div): Divides two **integers** and return the quotient and the remainder.
* Square root (sqrt): Calculates the square root of a decimal number and returns the result.

In addition, it supports to trace the desired operations within a journal that could be retrieved using the following operation:

* Journal: Given a tracking ID it returns all tracked operations.

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

```
dotnet build -c Release
```

### CalculatorService.Client

From startup project folder: /src/Client/CalculatorService.Client:

```
dotnet build -c Release
```

## Tests

From solution folder: /src

```
dotnet test
```

It will run 3 test projects:

* Unit tests
  * CalculatorService.Server.UnitTests
  * CalculatorService.Client.UnitTests
* Integration tests
  * CalculatorService.Server.IntegrationTests

## Deploy Server

### Deploy As Code

From solution folder: /src

```
dotnet restore "./Api/CalculatorService.Server/CalculatorService.Server.csproj"
```
```
dotnet build "./Api/CalculatorService.Server/CalculatorService.Server.csproj" -o <build_folder>
```
```
dotnet publish "./Api/CalculatorService.Server/CalculatorService.Server.csproj" -c Release -o <publish_folder/p:UseAppHost=false
```

To run the application from <publish_folder>:
```
dotnet CalculatorService.Server.dll
```

### Deploy As Container

From server project folder: /src/Api/CalculatorService.Server:

```
docker build -f Dockerfile .. -t <registry>:<tag>
```
```
docker push <registry>:<tag>
```
```
docker image pull <registry>:<tag>
```
```
docker run <registry>:<tag>
```

Docker file needs to be built from Linux/WSL2 or use Dock Desktop instead.

## Server API reference

The API reference documententation will be available using:

* SwaggerUI: https://localhost:5001/swagger/index.html
* OpenAPI specification v3.0.1: https://localhost:5001/swagger/v1/swagger.json

### Serialization formats

All endpoints accept JSON or XML formats:

* Requests: By setting Content-Type header to "application/json" or "application/xml"
* Responses: By setting optional "format" query parameter to "json" or "xml (i.e. /calculator/add?format=xml). Default response format is JSON.

### Logging

Server API logs requests and responses information to console and file. For logging to file it uses NLog implementation using daily file rotation. It keeps up to 100 log files. To modify NLog configuration use nlog.config file.

## Client Usage

### Publish client

From solution folder: /src

```
dotnet restore "./Client/CalculatorService.Client/CalculatorService.Client.csproj"
```
```
dotnet build "./Client/CalculatorService.Client/CalculatorService.Client.csproj" -o <build_folder>
```
```
dotnet publish "./Client/CalculatorService.Client/CalculatorService.Client.csproj" -c Release -o <publish_folder/p:UseAppHost=false
```

### Client configuration

From <publish_folderfolder, edit "appSettings.json" file and set the value of "API:BaseAddress". (i.e. https://localhost:5001)

### Run client

To run the application from <publish_folder>:
```
dotnet CalculatorService.Client.dll
```

The system will run a command prompt, enter "help" to see available commands, or enter a command name to get command specific help:

```
calc> help
```

Available commands: add, sub, mult, div, sqrt, journal

## Journal storage

The current implementation stores the tracked operations in memory, so all the stored data will be lost once the Server is shut down.

Other storages such databases or file system could be added implementing the interface "ITrackerService\<T\>"