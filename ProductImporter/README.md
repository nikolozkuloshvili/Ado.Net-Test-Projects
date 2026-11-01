# ProductImporter

`ProductImporter` is a .NET console application that imports tab-separated product data into a SQL Server `ProductsCatalogue` database.

## Requirements

- .NET 10 SDK
- SQL Server with Windows authentication enabled

## Database setup

1. Run `DBConfig/Database.sql` to create the `ProductsCatalogue` database and its tables.
2. Run `DBConfig/Import_SP.sql` to create the import stored procedure.
3. Update `DBConfig/appsettings.json` if your SQL Server instance is not the default local instance:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=ProductsCatalogue;Integrated Security=True;TrustServerCertificate=True;"
  }
}
```

The application loads this file from its output directory, so copy it alongside the published executable when deploying manually.

## Input format

The application expects a path to a UTF-8 text file. Each line must contain seven tab-separated fields:

```text
CategoryName    CategoryIsDeleted    ProductCode    ProductName    Price    Quantity    ProductIsDeleted
```

`CategoryIsDeleted` and `ProductIsDeleted` must be `0` or `1`. `Price` must be a positive decimal value, and `Quantity` must be a non-negative integer.

Example:

```text
Laptops    0    LAP-001    Example Laptop    1299.99    5    0
Accessories    0    ACC-001    USB-C Hub    49.99    20    0
```

## Run

From the repository root:

```bash
dotnet run --project ProductImporter/ProductImporter.csproj
```

When prompted, enter the path to the input file. The importer processes all valid records in a transaction and writes parsing or database errors to a dated file under the application's `logs/` directory.
