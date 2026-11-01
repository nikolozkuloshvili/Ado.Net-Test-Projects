using ProductImporter.Shared;

namespace ProductImporter.ProductReader;

public class CategoryDataReader : DataReader<Category>
{
    public CategoryDataReader(string path)
        : base(path)
    {
    }

    public CategoryDataReader(Stream stream)
        : base(stream)
    {
    }

    public override IList<Category> ReadAll()
    {
        var categories = new List<Category>();

        var position = _stream.Position;

        var productReader = new ProductDataReader(_stream);
        var products = productReader.ReadAll();

        _stream.Seek(0, SeekOrigin.Begin);

        var reader = new StreamReader(_stream);
        HashSet<string> uniqueCategories = new();
        while (!reader.EndOfStream)
        {
            try
            {
                var category = reader.ReadLine()!
                    .Split('\t', StringSplitOptions.RemoveEmptyEntries)
                    .CategoryBuilder();

                if (uniqueCategories.Add(category.Name))
                {
                    category.AddRangeProducts(ref products);
                    categories.Add(category);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        _stream.Seek(position, SeekOrigin.Current);

        return categories;
    }
}
