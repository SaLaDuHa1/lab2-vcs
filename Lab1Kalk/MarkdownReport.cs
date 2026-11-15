using System;

/// <summary>
/// Новый узел сети: генерация отчёта в формате Markdown.
/// Подключается через интерфейс IReportGenerator без изменения ReportService.
/// </summary>
public class MarkdownReport : IReportGenerator
{
    private readonly string _outputPath;

    public MarkdownReport(string outputPath)
    {
        _outputPath = outputPath;
    }

    public void Generate(string data)
    {
        Logger.Info($"[MARKDOWN] Генерация отчёта в '{_outputPath}': {data}");
    }
}
