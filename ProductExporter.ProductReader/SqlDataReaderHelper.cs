using Microsoft.Data.SqlClient;
using ProductExporter.Shared;
using System.Globalization;

namespace ProductExporter.ProductReader;

public static class SqlDataReaderHelper
{
    public static Product GetProduct(this SqlDataReader reader)
    {
        string code = reader["Code"].ToString()!;
        string productName = reader["ProductName"].ToString()!;
        var price = Convert.ToDecimal(Convert.ToDecimal(reader["Price"]).ToString("0.00", CultureInfo.CurrentCulture));
        int quanitity = (int)reader["Quantity"];
        bool pIsDeleted = !((Convert.ToByte(reader["ProductStatus"]) == 1));

        return new Product(GetCategory(reader), code, productName, price, quanitity, pIsDeleted);
    }

    public static Category GetCategory(this SqlDataReader reader)
        => new Category(reader["CategoryName"].ToString()!, !(Convert.ToByte(reader["CategoryStatus"]) == 1));
}
