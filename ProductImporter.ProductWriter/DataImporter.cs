using DBConfig;
using Microsoft.Data.SqlClient;
using ProductImporter.ProductReader;
using ProductImporter.Shared;
using System.Data;

namespace ProductImporter.ProductWriter;

public class DataImporter : IDataImporter
{
    protected readonly Stream _stream;

    public DataImporter(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path, nameof(path));

        _stream = new FileStream(path, FileMode.Open, FileAccess.Read);
    }

    public DataImporter(Stream stream)
    {
        ValidateStream(stream);

        _stream = stream;
    }

    public bool Import()
    {
        try
        {
            using var connection = DatabaseConfig.GetConnection();
            var command = connection.CreateCommand();
            command.CommandText = "ImportCategoryWithProduct_SP";
            command.CommandType = CommandType.StoredProcedure;

            connection.Open();

            using var transaction = connection.BeginTransaction();
            command.Transaction = transaction;


            var reader = new CategoryDataReader(_stream);
            var categories = reader.ReadAll();

            foreach (var category in categories)
            {
                try
                {
                    foreach (var product in category.Products)
                    {
                        command.Parameters.Clear();
                        command.ProductToSqlFormat(product);
                        command.ExecuteNonQuery();
                    }
                }
                catch
                {
                    $"Issue happend on Category with name {category.Name}".WriteToLogFile();
                    transaction.Rollback();
                    throw;
                }
            }

            transaction.Commit();
            Console.WriteLine("Operation was Successful.");

            return true;
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Operation was Unsuccessful.");
            Console.WriteLine("Please check Logs.txt");
            ex.Message.WriteToLogFile();

            return false;
        }
        catch (Exception)
        {
            Console.WriteLine("Operation was Unsuccessful.");
            Console.WriteLine("Please check Logs.txt");
            throw;
        }
    }

    private static void ValidateStream(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(stream.Length, 0);

        if (!stream.CanRead)
            throw new ArgumentException("Stream cannot be read.");

        if (!stream.CanSeek)
            throw new ArgumentException("Stream cannot seek.");
    }
}
