namespace ProductImporter.Shared;

public sealed class Product
{
    public Product(Category category, string code, string name, decimal price, int quantity, bool isDeleted)
    {
        ArgumentNullException.ThrowIfNull(category);
        ValidateNameAndCode(name, code);
        ValidatePriceAndQuantity(price, quantity);

        Code = code;
        Name = name;
        Price = price;
        Quantity = quantity;
        IsDeleted = isDeleted;
        Category = category;
    }

    public Category Category { get; }
    public string Code { get; }
    public string Name { get; }
    public decimal Price { get; }
    public int Quantity { get; }
    public bool IsDeleted { get; }

    public override string ToString() =>
        $"{Category.Name}\t{Convert.ToByte(Category.IsDeleted)}\t{Code}\t{Name}\t{Price}\t{Quantity}\t{Convert.ToByte(IsDeleted)}";

    private static void ValidatePriceAndQuantity(decimal price, int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(price, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(price, 0);
    }

    private static void ValidateNameAndCode(string name, string code)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
    }
}
