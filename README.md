# ADO.NET Product Importer


ProductImporter is a .NET console application designed to import tab-separated product data efficiently into a SQL Server ProductsCatalogue database using ADO.NET and stored procedures.
## Requirements

- .NET 10 SDK
- SQL Server with Windows authentication enabled
## Project structure

- `ProductImporter.Shared` - domain models (`Category`, `Product`) and parsing/log helpers.
- `ProductImporter.ProductReader` - stream/file readers that parse tab-delimited rows into models.
- `ProductImporter.ProductWriter` - `DataImporter` that sends parsed products to SQL Server.
- `DBConfig` - SQL connection configuration and SQL scripts for database/procedure creation.
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

## How the SQL import procedure works

`ImportCategoryWithProduct_SP` applies category + product rules per incoming row:

- If category does not exist:
  - Inserts category
  - Inserts product
  - If `CategoryIsDeleted = 1`, inserted product is also marked deleted
- If category exists and `CategoryIsDeleted = 1`:
  - Inserts current product as deleted
  - Marks category as deleted
  - Marks **all** products under that category as deleted
- If category exists and `CategoryIsDeleted = 0`:
  - Reactivates category (`IsDeleted = 0`)
  - Inserts product with incoming `ProductIsDeleted` flag
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
