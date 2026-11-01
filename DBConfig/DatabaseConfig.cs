using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DBConfig;

public static class DatabaseConfig
{
    public static SqlConnection GetConnection() => new SqlConnection(GetConnectionString());

    private static string? GetConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        return configuration.GetConnectionString("DefaultConnection");
    }
}
