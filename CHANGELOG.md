# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]

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
