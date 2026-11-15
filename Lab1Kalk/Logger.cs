using System;
using System.IO;

/// <summary>
/// Простой модуль логирования: пишет сообщения в консоль и в файл logs/app.log.
/// </summary>
public static class Logger
{
    private static readonly object SyncRoot = new();
    private static readonly string LogDirectory = Path.Combine(AppContext.BaseDirectory, "logs");
    private static readonly string LogFile = Path.Combine(LogDirectory, "app.log");

    public static void Info(string message) => Write("INFO", message);

    public static void Warn(string message) => Write("WARN", message);

    public static void Error(string message) => Write("ERROR", message);

    private static void Write(string level, string message)
    {
        var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";

        lock (SyncRoot)
        {
            Console.WriteLine(line);
            Directory.CreateDirectory(LogDirectory);
            File.AppendAllText(LogFile, line + Environment.NewLine);
        }
    }
}
