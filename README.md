# GloboticketWeb - A Demo Application

This project is a demonstration application created for the Refactor and Optimize Code with GitHub Copilot Pluralsight course by Mark Heath. It is not designed as a "best practices" example, but simply to contain some demo code that can be used as a starting point for using Copilot to perform refactoring.

Note that Git tags have been used for some of the checkpoints before and after various demos in the course, so use those to get to a specific point in time..

## Prerequisites

Before you begin, ensure you have the following installed:

*   [.NET SDK 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
*   [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/) with C# extension

## GitHub Copilot Extension

Also, in order to try out refactoring ideas, make sure you have the GitHub Copilot extension installed. You can download it from [GitHub Copilot](https://github.com/features/copilot).

## Getting Started

### Building the Project

To build the project, navigate to the `GloboticketWeb` directory in the terminal and run the following command:

```sh
dotnet build
```

Alternatively, you can build the project using Visual Studio. The project file is `GloboticketWeb.csproj`.

### Running the Application
To run the application, navigate to the `GloboticketWeb` directory and execute:

```sh
dotnet run
```

Or, run the application from within Visual Studio. The main program file is `Program.cs`.

The application will be accessible at https://localhost.

### Running Tests

To run the unit tests, navigate to the `GloboticketWeb.Tests` directory and run:

```sh
dotnet test
```

Alternatively, you can run the tests using the Test Explorer in Visual Studio. The test project file is `GloboticketWeb.Tests.csproj`.

## Restoring Libraries with LibMan
This project uses LibMan to manage client-side libraries. If the libraries are not included in your downloaded version, you can restore them using the following steps:

### Installing LibMan
If you don't have LibMan installed, you can install it using the following command:

```sh
dotnet tool install -g Microsoft.Web.LibraryManager.Cli
```

### Restoring Libraries

1. Open the project in Visual Studio.
1. Right-click on the GloboticketWeb project in the Solution Explorer.
1. Select Restore Client-Side Libraries.

Alternatively, you can restore the libraries via the command line:

1. Navigate to the GloboticketWeb directory.
1. Run the following command:
    ```dotnet libman restore```

This will restore the libraries defined in the `libman.json` file.