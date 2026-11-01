using Microsoft.Data.SqlClient;
using ProductImporter.Shared;
using System.Data;

namespace ProductImporter.ProductReader;

public static class SqlCommandHelper
{
    public static void ProductToSqlFormat(this SqlCommand command, Product product)
    {
        command.Parameters.Add("@CategoryName", SqlDbType.NVarChar, 100).Value = product.Category.Name;
        command.Parameters.Add("@CategoryIsDeleted", SqlDbType.Bit).Value = product.Category.IsDeleted;
        command.Parameters.Add("@ProductName", SqlDbType.NVarChar, 100).Value = product.Name;
        command.Parameters.Add("@Code", SqlDbType.VarChar, 10).Value = product.Code;
        command.Parameters.Add("@Price", SqlDbType.Money).Value = product.Price;
        command.Parameters.Add("@Quantity", SqlDbType.Int).Value = product.Quantity;
        command.Parameters.Add("@ProductIsDeleted", SqlDbType.Bit).Value = product.IsDeleted;
    }
}
