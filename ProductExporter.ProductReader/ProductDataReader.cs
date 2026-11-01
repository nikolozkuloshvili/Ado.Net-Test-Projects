using DBConfig;
using Microsoft.Data.SqlClient;
using ProductExporter.Shared;
using System.Data;

namespace ProductExporter.ProductReader
{
    public class ProductDataReader
    {
        public IList<Product> GetProducts()
        {
            List<Product> products = new();

            using var connection = DatabaseConfig.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "select * from GetProductsByCategoryName_VW";
            command.CommandType = CommandType.Text;

            using SqlDataReader sqlReader = command.ExecuteReader();

            while (sqlReader.Read())
            {
                products.Add(sqlReader.GetProduct());
            }

            return products;
        }
    }
}
