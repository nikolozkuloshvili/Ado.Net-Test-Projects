using ProductExporter.ProductWriter;

namespace ProductExporter;

internal class Program
{
    static void Main()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("Please enter path where to save the exported data: ");
                var exporter = new DataExporter(Console.ReadLine()!);

                if (exporter.Export())
                {
                    Console.WriteLine("Operation was Successful");
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Please try again.");
                continue;
            }
        }
    }
}

