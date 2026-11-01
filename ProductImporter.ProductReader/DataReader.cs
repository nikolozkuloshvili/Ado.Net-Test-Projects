namespace ProductImporter.ProductReader;

public abstract class DataReader<T> : IDataReader<T>
{
    protected readonly Stream _stream;

    public DataReader(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path, nameof(path));

        _stream = new FileStream(path, FileMode.Open, FileAccess.Read);
    }

    public DataReader(Stream stream)
    {
        ValidateStream(stream);

        _stream = stream;
    }

    public abstract IList<T> ReadAll();

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
