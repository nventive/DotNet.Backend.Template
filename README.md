# NV.Templates

.NET Templates for backend applications and open-source components.

This project provides templates that include backed-in patterns and best practices based on experience of implementing many solutions using Microsoft technologies.

Generated projects can:
- Build a Web project that exposes a REST API, a GraphQL API, host a SPA application, run in Azure Functions or as a command-line application
- Create netstandard OSS components published on NuGet/GitHub

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)
[![Build Status](https://dev.azure.com/nventive-public/nventive/_apis/build/status/nventive.NV.Templates?branchName=master)](https://dev.azure.com/nventive-public/nventive/_build/latest?definitionId=6&branchName=master)
![Nuget](https://img.shields.io/nuget/v/NV.Templates.svg)

## Getting Started

### Prerequisites

- Install the [.NET Core SDK](https://dotnet.microsoft.com/download)

- Install the templates locally
```shell
dotnet new -i NV.Templates
```

### Generate a Backend project

Run the template with the relevant options:
```shell
dotnet new nv-backend -n <project name> -c <company name> [options]
```

By default, the project will only generate a `Core` library to host the
core business logic of the application.
To generate suitable hosts to run and expose it, use the following options:

| Option      | Description                                                           |
|-------------|-----------------------------------------------------------------------|
| --RestApi   | Generates an ASP.NET Core MVC project suitable for exposing REST APIs |
| --GraphqlApi| Generates an ASP.NET Core project suitable for exposing GraphQL APIs  |
| --SPA       | Generates an ASP.NET Core project suitable for hosting SPAs           |
| --Functions | Generates an Azure Functions project                                  |
| --Console   | Generates a Console (command-line) project                            |
| --Auth      | Add authentication (JWT-based) support in Web projects                |
| --EFCore    | Add EF Core support                                                   |

Options can be combined, e.g.
```shell
dotnet new nv-backend -n <project name> -c <company name> --RestApi --SPA --Functions --Console --Auth
```

For more details on each project type, see the [Features / Backend](#backend) section.

### Generate a netstandard Component project

Run the template:
```shell
dotnet new nv-netstandard-component -n <component name> -c <company name>
```

For more details on what's provided, see the [Features / Component](#component) section.

## Features

### Backend

#### Core

Regardless of the options provided to the template, there is always 2 projects
that are generated: `Core` and `Core.Tests`, along with some supporting solution files.

Here is the set of feature provided as part of the Core feature set:

- `Core`: a `netstandard2.0` library where the business logic of the application
is meant to reside. Contains:
  - `IApplicationInfo` / `ApplicationInfo` service that represents the execution
    environment of the application itself
  - `IOperationContext` / `OperationContext` service that represents a the execution
    environment of a single operation executed by the library; it is meant to be
    registered as a *Scoped* service, and the lifetime of it must be managed by the
    Dependency Injection container
  - Various interfaces and base classes for Entity-type class (in the `Framework` namespace) 
  - an `IdGenerator` utility class to generate unique ids as reasonably readable strings
  - Inclusion of [FluentValidation](https://fluentvalidation.net/) as the preferred
    library for validating entities
  - Inclusion of helpers and extension methods to handle pagination as Continuation tokens
    (in `Framework/Continuation`) as the preferred method to handle pagination
    (as opposed to offset pagination)
  - A set of standard, application-level `Exception` classes to handle common cases
    (`ConcurrencyException`, `DependencyException`, `NotFoundException`)
  - An extension method to help registering all services in the Dependency Injection
    container (in `ServiceCollectionExtensions`)
  - A set of attributes that helps with auto-registration of services (see the `OperationContext` class for an example)

- `Core.Tests`: a [xUnit](https://xunit.net/) project for unit-tests. Contains:
  - `OptionsHelper` to help load [Options](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-2.2) from unit-tests projects configuration
  - `EntityFactory` to help implement the [Object Mother](https://martinfowler.com/bliki/ObjectMother.html) unit testing pattern
  - An adapter that adapts a `ILogger` to the standard xUnit output

- Solution Items files: Various solution files that apply to all projects:
  - an [.editorconfig](https://editorconfig.org/) configuration file
  - standard configuration for [StyleCop](https://github.com/DotNetAnalyzers/StyleCopAnalyzers) and [FxCop](https://github.com/dotnet/roslyn-analyzers) that applies to all projects in the solution
  - common set of properties for all projects (in `Directory.Build.props`)
  - all projects are marked with a property named `TemplateVersion` that is set
    to the template version used during project generation
  - a Powershell script to generate an `ATTRIBUTIONS.txt` file to collect all
    NuGet packages license information to ensure compliance

*Implementation Get Started*

- Create the relevant service interfaces and implementations; services can be annotated with `[RegisterXXXService]` attributes to be auto-registered in Dependency Injection
- Create the corresponding unit tests in the `Core.Tests` project

*Using the library*

In order to use the library in the context of an application, it needs to be setup
in a specific way. While the template options for head projects provide such an
environment, if you need to do it yourself here is what's needed:

- [Create a .NET Host](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-2.2)
- Configure the library by adding the library services using the `IServiceCollection.AddCore` extension method
- Ensure that each operation is executed in a separate scope
  (e.g. by using [`IServiceProvider.CreateScope`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.serviceproviderserviceextensions.createscope?view=aspnetcore-2.2))


#### RestApi

When using the `--RestApi` option, 2 projects are added to the solution:

- `Web`: An ASP.NET Core application setup for exposing the Core library as a Rest API. Contains the following features:
  - MVC bootstrap code with API-related features only (using [AddMvcCore](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.mvccoreservicecollectionextensions.addmvccore?view=aspnetcore-2.2) instead of [AddMvc](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.mvcservicecollectionextensions.addmvc?view=aspnetcore-2.2))
  - JSON options configured with sensible default
  - [ASP.NET Core API Versioning](https://github.com/microsoft/aspnet-api-versioning) is
    enabled and setup with support for versioning of the API in the URL (e.g. `/api/v1/...`)
  - [FluentValidation](https://fluentvalidation.net/) is configured and integrated with
    the ASP.NET Core MVC pipeline
  - [Application Insights](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview) is configured
  - HTTPS is enforced and [HSTS](https://fr.wikipedia.org/wiki/HTTP_Strict_Transport_Security) is configured
  - [CORS](https://developer.mozilla.org/fr/docs/Web/HTTP/CORS) is configured
  - Full request and response tracing is available, courtesy of the `AspNetCoreRequestTracing` component
  - Generic error handling middleware is already setup (in `Framework.Middleware.ExceptionHandler`)
    and configured for the exception classes provided in the Core project
  - HTTP Response caching is configured to return "no-store,no-cache" by default
  - Swagger/Open API support is configured, courtesy of [NSwag](https://github.com/RicoSuter/NSwag)
  - `/attributions.txt` handler configured, to expose the `ATTRIBUTIONS.txt` file
    the includes 3rd-party NuGet licenses
  - `/info` endpoint that exposes `IApplicationInfo` information
  - [ASP.NET Core health checks](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-2.2) are configured, with additional tracing options exposed through a `/health` endpoint
  - Models that maps with the Continuation tokens classes defined in `Core`
  - all operations return a `X-OperationId` header with the current `IOperationContext.Id` value
  - errors include an additional Help Desk Id property, courtesy of the `HelpDeskId` component

- `Web.Tests`: a [xUnit](https://xunit.net/) [integration tests](https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-2.2) project.
  Sets up a `TestWebApplicationFactory` and a xUnit collection for tests that 
  automatically starts an in-memory instance of the ASP.NET Core application.

#### GraphQL

When using the `--GraphqlApi` option, the `Web` and `Web.Tests` projects described above is augmented with the following features:

- `Web`: 
  - Full GraphQL Query and response tracing is available
  - Support for GraphQL Extensions that output `IOperationContext` information
  - Generic error handling Field Middleware
  - Support for custom error handling that include an additional Help Desk Id property, courtesy of the `HelpDeskId` component
  - Built-in [GraphiQL](https://github.com/graphql/graphiql) and [GraphQL Voyager](https://github.com/APIs-guru/graphql-voyager) endpoints
  - Support for `IDependencyResolver` in GraphQL nodes to resolve dependencies
  - Custom Models for continuation collections and continuation tokens

- `Web.Tests`:
  Sets up a `TestWebApplicationFactory` and a xUnit collection for tests that 
  automatically starts an in-memory instance of the ASP.NET Core application and
  configures a `GraphQLClient`.

#### SPA

When using the `--SPA` option, the `Web` project is augmented with the following features:

- Adds support for ASP.NET Core SPA Services, with on-the-fly compilation and hot-reload when developing and serving the statically compiled app when hosting in production
- Creates a `ClientApp` folder that should host the content of the SPA; it is voluntarily left empty

Once the solution has been generated, go to the `Web\ClientApp` folder and generate the SPA client app here using the SPA tooling of your choice (e.g. [Create React App](https://create-react-app.dev/) or [Angular CLI](https://cli.angular.io/)).

#### Auth

When using the `--Auth` option, the `Web` project is augmented with the following features:

- Adds support for validating JWT tokens
- Configuration is achieved via `AuthenticationOptions`; an example is given in `appsettings.Development.json`
- Integration with RestApi and GraphQLApi
- Integration with the `IOperationContext`
- Integration with the Application Insights Telemetry
- Adds support for OAuth2 login in Open API

Covering the entire scope of authentication and authorization is too large for this documentation.
This option is only there to provide a starting point.
Please refer to the [ASP.NET Core documentation](https://docs.microsoft.com/en-us/aspnet/core/security/?view=aspnetcore-2.2) for more info.

#### EF Core

When using the `--EFCore` option, the `Core` and `Core.Tests` projects are augmented with the following features:

- Adds a baseline `CoreDbContext` with client evaluation disabled and configuration via `IEntityTypeConfiguration` enabled
- Adds some `IQueryable` extension methods to support `NotFoundException` and `ContinuationTokenEnumerable`
- Adds a `CoreDbContextHelper` that helps with SQLite in-memory unit tests support

The `CoreDbContext` class is intentionnaly marked as `internal` so as it does not blead to peripheral head projects.
In order to use the EF Core tooling, you must specify a `--startup-project` when running the commands, e.g.:

```shell
dotnet ef database update --project <Core project> --startup-project <Web project>
```

Additionaly, the project adds a ``docker-compose.yml` that allows you to:

- Start a docker container with MS SQL Server running locally on linux with a new database created
- Easily restore any additional database on container start by dropping the backups into the `.docker/mssql/to-restore` folder

The `Web` and `Functions` projects are configured to use this database in development. Just run `docker-compose up` and you're good to go.

#### Azure Functions

When using the `--Functions` option, 2 projects are added to the solution:

- `Functions`: an Azure Functions project
  - Azure Functions setup with [integrated Dependency Injection](https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection)
  - Support for scoped execution is [provided by the Azure Functions SDK](https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection#service-lifetimes)


- `Functions.Tests`: a [xUnit](https://xunit.net/) project for unit testing the functions

#### Console app

When using the `--Console` option, 2 projects are added to the solution:

- `Console`: a .NET Core console application
  - Organized around the Command pattern using [`CommandLineUtils`](https://github.com/natemcmaster/CommandLineUtils)
  - Sets up a Generic Host with integrated Dependency injection
  - Each command is a separate class, with argument parsing

- `Console.Tests`: a [xUnit](https://xunit.net/) project for unit testing the commands

### NetStandard Component

Provides a base repository, solution and projects to create an open-source component.

Includes:

- an [.editorconfig](https://editorconfig.org/) configuration file
- 2 projects:
  - a `netstandard2.0` library for the component
  - a [xUnit](https://xunit.net/) project for unit testing
- standard configuration for [StyleCop](https://github.com/DotNetAnalyzers/StyleCopAnalyzers) and [FxCop](https://github.com/dotnet/roslyn-analyzers) that applies to all projects in the solution
- common set of properties for all projects (in `Directory.Build.props`)
- all projects are marked with a property named `TemplateVersion` that is set
  to the template version used during project generation
- default `azure-pipeline.yml` CI build file, along with an adequate [GitVersion](https://gitversion.readthedocs.io) configuration
- standard supporting file for GitHub-published components:
  - a default `README.md` file
  - a [`CHANGELOG.md`](https://keepachangelog.com) file
  - default GitHub Issue and Pull Requests templates
  - an Apache-2.0 `LICENSE` file
  - a [Contributor Covenant Code of Conduct](https://www.contributor-covenant.org/)
  - instructions regarding contributions

## Changelog

Please consult the [CHANGELOG](CHANGELOG.md) for more information about version
history.

## License

This project is licensed under the Apache 2.0 license - see the
[LICENSE](LICENSE) file for details.

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on the process for
contributing to this project.

Be mindful of our [Code of Conduct](CODE_OF_CONDUCT.md).

## Acknowledgments

- [Dotnet-Boxed Templates](https://github.com/Dotnet-Boxed/Templates)
