namespace ProductExporter.ProductReader;

public interface IDataReader<T>
{
    public IList<T> GetCategories();
}