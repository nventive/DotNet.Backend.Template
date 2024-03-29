# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]

 - Migrate from 3.1 to .net 6  Finalize package reference to lts

## 2.4.1

### Added

- Add default build/release multistage pipeline
- Add default AppConfiguration setup in the code
- Add XML serializer for HTTP clients
- Add ability to ignore SSL certificates validation for dependencies
- Add default Keyvault setup in the code with user secret for local development
- Add default Keyvault with access policies and system managed identity

### Changed

- Adjust default HTTP headers to remove unsecured headers or refine required ones
- Update api vesion in ARM template for schema, components and referenceId functions
- Migrate from 3.1 to .net 6 ( Web / Core/ AzFunction)

### Deprecated

### Removed

## 2.2.1

### Added

- Azure ARM Template
- Azure DevOps Build Pipeline
- HttpDependencies / Refit w/HttpClientOptions support.
- Multiple ApiVersion support for NSwag
- Ability to disable Swagger from configuration
- Default in memory distributed cache
- Ability to setup Redis cache to replace the default in memory distributed cache

### Changed

- Added support for .Net Core 3.1 & C# 8
- Removed appsettings.json files in favor of a unified `LocalSettings.Development.json` & `LocalSettings.Test.json` file outside the projects.
- Updated HttpTracing & HttpRecorder dependencies
- Core Tests use a TestHost now to retrieve services

### Deprecated

### Removed

- Base Entity & IIdentifiable implementations
- GraphQL support - waiting for .Net Core 3 overall from graphql-dotnet
- EF Core Support
- FluentValidation

### Fixed

### Security

## 2.2.0

### Added

- Support for EF Core & MS SQL Docker

### Changed

### Deprecated

### Removed

### Fixed

### Security

## 2.1.0

### Added

- Support for alternative authentication in Integration Tests when using `--Auth` option.

### Changed

- Default JWT Auth configuration uses `DefaultScheme` instead of `DefaultAuthenticateScheme`

### Deprecated

### Removed

### Fixed

- Loading configuration options for `RequestTracingMiddlewareOptions`

### Security

## 2.0.0

### Added

- Added LimitOffsetContinuationToken (ContinuationToken implementation using Limit & Offset pagination information)
- Added Auto-registration for services
- Added ClientApp SPA support
- Added Authentication support via JWT
- Added UserSecrets default configuration and support in `OptionsHelper`

### Changed

- Merged RestApi & GraphQLApi into a single Web project
- Updated NuGet dependencies (to the exclusion of anything .Net Core 3-related)
- Renamed nv-component template to nv-netstandard-component

### Deprecated

### Removed

- Removed NodaTime default support - Users will be able to add it if they want to, but it's not longer added by default to reduce OOB complexity.

### Fixed

### Security

## 1.0.0

### Added

- Initial version:
    - Backend: Core, ASP.NET Core, GraphQL, Functions, Console
    - Component

### Changed

### Deprecated

### Removed

### Fixed

### Security
