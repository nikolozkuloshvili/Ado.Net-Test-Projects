namespace ProductExporter.Shared;

public sealed class Category
{
    private readonly List<Product> _products = new();

    public Category(string name, bool isDeleted)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));

        Name = name;
        IsDeleted = isDeleted;
    }

    public Category(string name, bool isDeleted, List<Product> products)
        : this(name, isDeleted)
    {
        ArgumentNullException.ThrowIfNull(products, nameof(products));
        _products = products;
    }

    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    public string Name { get; }
    public bool IsDeleted { get; }

    public bool AddProduct(Product product)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(product);

            if (product.Category!.Name != this.Name)
                throw new ArgumentException($"Product category name '{product.Category.Name}' does not match this category name '{this.Name}'.");

            if (_products.Contains(product))
                throw new ArgumentException($"This product already exists in this category.");

            _products.Add(product);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public void AddRangeProducts(ref IList<Product> products)
    {
        for (int i = 0; i < products.Count; i++)
        {
            if (AddProduct(products[i]))
            {
                products.RemoveAt(i);
                i--;
            }
        }
    }

    public override string ToString() => $"{Name}\t{Convert.ToByte(IsDeleted)}";
}