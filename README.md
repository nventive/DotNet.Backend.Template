# NV.Templates

.NET Core Templates for backend applications and open-source components.

This project provides templates that include backed-in patterns and best practices
that complements the base .NET Core framework, based on experience of implementing
many backend solutions using Microsoft technologies.

Generated projects can expose a REST API, a GraphQL API, run in Azure Functions or
as a command-line application.

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)

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
| --restapi   | Generates an ASP.NET Core MVC project suitable for exposing REST APIs |
| --graphql   | Generates an ASP.NET Core project suitable for exposing GraphQL APIs  |
| --functions | Generates an Azure Functions project                                  |
| --console   | Generates a Console (command-line) project                            |

Options can be combined to generate multiple hosts, e.g.
```shell
dotnet new nv-backend -n <project name> -c <company name> --restapi --functions --console
```

For more details on each project type, see the [Features / Backend](#backend) section.

### Generate a Component project

Run the template:
```shell
dotnet new nv-component -n <component name> -c <company name>
```

For more details on what's provided, see the [Features / Component](#component) section.

## Features

### Backend

### Component

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
