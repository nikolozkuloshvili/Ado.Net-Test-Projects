# ADO.NET Product Exporter

ProductExporter is a .NET console application designed to export product data from a SQL Server `ProductsCatalogue` database into a tab-separated text file using ADO.NET.

## Requirements

- .NET 10 SDK
- SQL Server with Windows authentication enabled

## Project structure

- `ProductExporter.Shared` - domain models (`Category`, `Product`) and logging helper.
- `ProductExporter.ProductReader` - SQL readers that load categories and products from database views.
- `ProductExporter.ProductWriter` - `DataExporter` that writes exported rows to a file stream.
- `ProductExporter` - console entry point.
- `DBConfig` - SQL connection configuration and SQL scripts for database/view creation.

## Database setup

1. Run `DBConfig/DataBase.sql` to create the `ProductsCatalogue` database and its tables.
2. Run `DBConfig/GetCategories_VW.sql` to create the categories view.
3. Run `DBConfig/GetProductsByCategoryName_VW.sql` to create the products view.
4. Update `DBConfig/appsettings.json` so the connection string matches your SQL Server instance and database name:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=ProductsCatalogue;Integrated Security=True;TrustServerCertificate=True;"
  }
}
```

The application loads this file from its output directory, so keep `appsettings.json` alongside the built executable when deploying manually.

## How export works

`DataExporter` performs the export in these steps:

- Reads categories from `GetCategories_VW`
- Reads products from `GetProductsByCategoryName_VW`
- Matches products to categories in memory
- Writes each product as a tab-separated row in this format:

```text
CategoryName    CategoryIsDeleted    ProductCode    ProductName    Price    Quantity    ProductIsDeleted
```

## Run

From the repository root:

```bash
dotnet run --project ProductExporter/ProductExporter.csproj
```

When prompted, enter the destination path where export data should be saved. If the provided path does not point to an existing file, the exporter creates a file by appending `Exported.txt` to the provided value.

Database-related errors are written to a dated file under the application's `logs/` directory.
