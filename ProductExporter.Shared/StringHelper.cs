namespace ProductExporter.Shared;

public static class StringHelper
{
    public static void WriteToLogFile(this string exceptionMessage)
    {
        string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        Directory.CreateDirectory(logDirectory);

        string fileName = $"Logs_{DateTime.Now:yyyy-MM-dd}.txt";
        string logFilePath = Path.Combine(logDirectory, fileName);

        using var logFile = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
        using var writer = new StreamWriter(logFile);

        writer.WriteLine();
        writer.WriteLine($"{exceptionMessage} {DateTime.Now}");
    }
}
