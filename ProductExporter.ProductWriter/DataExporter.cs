using ProductExporter.ProductReader;

namespace ProductExporter.ProductWriter;

public class DataExporter
{
    protected readonly Stream _stream;

    public DataExporter(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path, nameof(path));

        if (!File.Exists(path))
        {
            path += "Exported.txt";
            _stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            return;
        }

        _stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
    }

    public DataExporter(Stream stream)
    {
        ValidateStream(stream);

        _stream = stream;
    }

    public bool Export()
    {
        try
        {
            using var writer = new StreamWriter(_stream);

            var categories = new CategoryDataReader().GetCategories();

            ArgumentNullException.ThrowIfNull(categories);

            foreach (var category in categories)
            {
                foreach (var product in category.Products)
                {
                    writer.WriteLine(product.ToString());
                }
            }

            return true;
        }
        catch (Exception)
        {
            Console.WriteLine("Operation was Unsuccessful.");
            throw;
        }
    }

    private static void ValidateStream(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(stream.Length, 0);

        if (!stream.CanWrite)
            throw new ArgumentException("Stream cannot be written.");

        if (!stream.CanSeek)
            throw new ArgumentException("Stream cannot seek.");
    }
}
