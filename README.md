# nventive Backend project template

## Pre-requisite
Install [.Net Core SDK](https://dotnet.microsoft.com/download)

## Install template

- Run `dotnet new -i NV.Templates

## Create a backend project

Run `dotnet new nv-backend -n <project root namespace> -c <company name>`

The default project template only generates the Core project. To add head projects, use the following options:
  - `--aspnet`: Adds ASP.NET project head
  - `--graphql`: Adds GraphQL project head
  - `--functions`: Adds Azure Functions project head
  - `--console`: Adds Console project head

## Create a component project

Run `dotnet new nv-component -n <project root namespace> -c <company name>`

