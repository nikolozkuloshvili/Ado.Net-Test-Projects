using ProductImporter.Shared;

namespace ProductImporter.ProductReader;

public class ProductDataReader : DataReader<Product>
{
    public ProductDataReader(string path) :
        base(path)
    {
    }

    public ProductDataReader(Stream stream) :
    base(stream)
    {
    }

    public override IList<Product> ReadAll()
    {
        var products = new List<Product>();

        var position = _stream.Position;

        using var reader = new StreamReader(_stream, leaveOpen: true);

        _stream.Seek(0, SeekOrigin.Begin);
        while (!reader.EndOfStream)
        {
            try
            {
                products.Add(reader.ReadLine()!
                    .Split('\t', StringSplitOptions.RemoveEmptyEntries)
                    .ProductBuilder());
            }
            catch (Exception ex)
            {
                ex.Message.WriteToLogFile();
                $"Wrong format on line {_stream.Position}".WriteToLogFile();
            }
        }
        _stream.Seek(position, SeekOrigin.Current);

        return products;
    }
}
