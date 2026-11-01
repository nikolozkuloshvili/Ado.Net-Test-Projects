using DBConfig;
using Microsoft.Data.SqlClient;
using ProductExporter.Shared;
using System.Data;

namespace ProductExporter.ProductReader;

public class CategoryDataReader : IDataReader<Category>
{
    public IList<Category> GetCategories()
    {
        var categories = new List<Category>();

        try
        {
            using var connection = DatabaseConfig.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "select * from GetCategories_VW";
            command.CommandType = CommandType.Text;

            using SqlDataReader sqlReader = command.ExecuteReader();

            while (sqlReader.Read())
            {
                categories.Add(sqlReader.GetCategory());
            }

            var products = new ProductDataReader().GetProducts();

            foreach (var category in categories)
            {
                category.AddRangeProducts(ref products);
            }

            return categories;
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Please check Logs.txt");
            ex.Message.WriteToLogFile();
            return null!;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
