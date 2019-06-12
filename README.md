# nventive Backend project template

## Pre-requisite
Install [.Net Core SDK](https://dotnet.microsoft.com/download)

## Install template

- Clone this repository
- Run `dotnet new -i "<local template directory>\Template\Content"`

## Create a project

Run `dotnet new backend -n <project root namespace>`

The default project template only generates the Core project. To add head projects, use the following options:
  - `--aspnet`: Adds ASP.NET project head
  - `--functions`: Adds Azure Functions project head
  - `--console`: Adds Console project head
  - `--resourceGroup`: Adds Azure ARM Template (Resource Group) project
  - `--nodoc`: Disable Architecture Documentation generation

## Update the template to the latest version

- Git pull the local template repository
- Run `dotnet new -i "<local template directory>"` again

## Recipes

This projects also contains reference implementations and recipes for various topics.
Please check the Recipes solution for more.
