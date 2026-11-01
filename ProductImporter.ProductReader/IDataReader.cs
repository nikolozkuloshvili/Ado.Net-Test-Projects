namespace ProductImporter.ProductReader;

public interface IDataReader<T>
{
    public IList<T> ReadAll();
}
