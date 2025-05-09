# DepthCharts Coding Test

Please accept my submission for the DepthCharts coding challenge.

## Prerequisites

This project targets .NET 8 (LTS).
Please download and install the SDK from the [official Microsoft page](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

## Project Structure

### `src/DepthCharts.Domain`

Core domain logic, including entities and value objects.

### `src/DepthCharts.Application`

Application-specific use cases and orchestration logic.

### `src/DepthCharts.ConsoleApp`

CLI entry point to demonstrate functionality.

### `tests/DepthCharts.Tests`

Unit tests for domain logic and use cases.

## Command Line Run Instructions

To build and run the project from the command line:

```bash
dotnet build
dotnet run --project src/DepthCharts.ConsoleApp
```

This runs the sample console application, which demonstrates all required use cases (adding/removing players, retrieving the depth chart, etc.).

## Automated Tests

Automated tests are written using [xUnit](https://xunit.net/) and cover all required use cases, including:

- Adding players to the depth chart (with and without depth)
- Removing players
- Retrieving the full depth chart
- Getting players under a specified player
- Edge cases (duplicate players, invalid removals, multi-position players)

You can run the full test suite using:

```bash
./runtests.sh --watch --verbosity normal
```

The script uses a subshell to change into the correct directory for maximum compatibility. You can pass any valid dotnet test arguments.

## Developer Tooling

- Included a ~/.vscode/settings.json file for consistent formatting across the team, helping reduce noise in pull requests.

- Developed using C# Dev Kit in VS Code.

- In a team setting, I would adopt any existing coding standards — or propose a shared configuration to align the team.

## Coding Notes

- Positions are modeled using an enum to enforce type safety, prevent invalid string inputs, and keep the system easily extensible to support additional sports in the future.

- - The original scaffold used .NET 5, which is end-of-life. I upgraded the solution to .NET 8 to align with current LTS standards, ensuring long-term maintainability along with improved performance and security. (While .NET 9 is available, it is a short-term support (STS) release, whereas .NET 8 is LTS and supported until November 2026.)
