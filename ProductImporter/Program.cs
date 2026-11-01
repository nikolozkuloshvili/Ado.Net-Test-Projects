using ProductImporter.ProductWriter;

namespace G10_ProductImporter;

internal class Program
{
    static void Main()
    {
        while (true)
        {
            try
            {
                Console.Write("Please enter path of the file to import: ");
                var importer = new DataImporter(Console.ReadLine()!);

                if (importer.Import())
                    break;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Try again.");
                continue;
            }
        }
    }
}
