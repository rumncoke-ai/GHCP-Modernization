# LegacyShop

LegacyShop is a sample online store built with ASP.NET Core MVC. It demonstrates a traditional layered application while providing common store-management workflows for products, categories, customers, and orders.

## Features

- Browse products and view product details
- Manage product categories and customers
- Create, view, edit, and delete orders
- Manage products through the Admin area
- Persist store data in SQL Server with Entity Framework 6
- Run unit, controller, data, and integration tests with MSTest

## Solution structure

| Project | Purpose |
| --- | --- |
| `LegacyShop.Web` | ASP.NET Core MVC web application and user interface |
| `LegacyShop.Business` | Business services and application logic |
| `LegacyShop.Data` | Entity Framework 6 entities, repositories, and migrations |
| `LegacyShop.Tests` | Unit and integration tests |

## Prerequisites

Install the following before starting:

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQL Server Express with an instance named `SQLEXPRESS`, or another accessible SQL Server instance
- Visual Studio with ASP.NET/web development and SQL Server tooling if you want to apply the EF6 migration from Package Manager Console

Verify the .NET SDK:

```powershell
dotnet --version
```

The reported version should begin with `10.`.

## Getting started

### 1. Restore and build the solution

From the repository root, run:

```powershell
dotnet restore .\LegacyShop.sln
dotnet build .\LegacyShop.sln
```

### 2. Configure SQL Server

The default connection string is in `LegacyShop.Web\appsettings.json` and connects to:

```text
.\SQLEXPRESS
```

To use another SQL Server instance without editing the checked-in configuration, set the connection string for the current PowerShell session:

```powershell
$env:ConnectionStrings__LegacyShopContext = "Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=LegacyShop;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True"
```

Replace the value with the connection string for your environment when necessary.

### 3. Create the database schema

The application does not create or migrate the database automatically. Apply the included Entity Framework 6 migration before the first run.

In Visual Studio:

1. Open `LegacyShop.sln`.
2. Open **Tools > NuGet Package Manager > Package Manager Console**.
3. Run the following command, replacing the connection string when needed:

```powershell
Update-Database `
  -ProjectName LegacyShop.Data `
  -StartUpProjectName LegacyShop.Web `
  -ConnectionString "Data Source=.\SQLEXPRESS;Initial Catalog=LegacyShop;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True" `
  -ConnectionProviderName "System.Data.SqlClient"
```

Use the same database server and catalog in this command and in `ConnectionStrings:LegacyShopContext`.

### 4. Start the application

Run:

```powershell
dotnet run --project .\LegacyShop.Web\LegacyShop.Web.csproj
```

Open the HTTPS or HTTP URL printed in the terminal. The main navigation provides access to Products, Categories, Customers, Orders, and the Admin area.

If local HTTPS certificate validation fails, trust the ASP.NET Core development certificate and start the application again:

```powershell
dotnet dev-certs https --trust
```

Stop the application with <kbd>Ctrl</kbd>+<kbd>C</kbd>.

## Running tests

Run all tests from the repository root:

```powershell
dotnet test .\LegacyShop.sln
```

## Configuration

Application configuration is stored in `LegacyShop.Web\appsettings.json`.

| Setting | Description |
| --- | --- |
| `ConnectionStrings:LegacyShopContext` | SQL Server connection used by Entity Framework |
| `StoreName` | Display name reserved for the store |
| `Logging:LogLevel` | Application and ASP.NET Core logging levels |
| `AllowedHosts` | Hosts accepted by the ASP.NET Core host |

ASP.NET Core environment variables can override these values. Replace each `:` with `__`; for example, use `ConnectionStrings__LegacyShopContext` to override the database connection.

## Troubleshooting

### SQL Server connection errors

- Confirm that the configured SQL Server instance is installed and running.
- Confirm that the database connection used for the migration matches the application's connection string.
- If `.\SQLEXPRESS` is unavailable, use LocalDB or another SQL Server instance and override `ConnectionStrings__LegacyShopContext`.

### Missing tables

The application intentionally disables automatic database initialization. Apply the EF6 migration described in the setup steps before opening a page that reads store data.

### The required .NET SDK is not found

Install the .NET 10 SDK and confirm that `dotnet --list-sdks` includes a `10.x` version.
