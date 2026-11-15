using System;
using System.Collections.Generic;

// ============================================
// ШАГ 1. Абстракция (интерфейс)
// ============================================

// Интерфейс — центр сетевой структуры
public interface IReportGenerator
{
    void Generate(string data);
}

// ============================================
// ШАГ 2. Конкретные реализации (узлы сети)
// ============================================

// Узел 1: PDF-отчёт
public class PdfReport : IReportGenerator
{
    private readonly string _outputPath;

    public PdfReport(string outputPath)
    {
        _outputPath = outputPath;
    }

    public void Generate(string data)
    {
        // Здесь была бы реальная работа с библиотекой типа iTextSharp
        Logger.Info($"[PDF] Генерация отчёта в '{_outputPath}': {data}");
    }
}

// Узел 2: Excel-отчёт
public class ExcelReport : IReportGenerator
{
    private readonly string _outputPath;

    public ExcelReport(string outputPath)
    {
        _outputPath = outputPath;
    }

    public void Generate(string data)
    {
        Logger.Info($"[EXCEL] Генерация отчёта в '{_outputPath}': {data}");
    }
}

// Узел 3: HTML-отчёт
public class HtmlReport : IReportGenerator
{
    private readonly string _outputPath;

    public HtmlReport(string outputPath)
    {
        _outputPath = outputPath;
    }

    public void Generate(string data)
    {
        Logger.Info($"[HTML] Генерация отчёта в '{_outputPath}': {data}");
    }
}

// ============================================
// ШАГ 3. Основной модуль (слабая связность)
// ============================================

// Класс, реализующий основную логику — зависит от абстракций
public class ReportService
{
    private readonly IEnumerable<IReportGenerator> _generators;

    // Внедрение зависимостей через конструктор
    public ReportService(IEnumerable<IReportGenerator> generators)
    {
        _generators = generators;
    }

    public void GenerateAll(string data)
    {
        Logger.Info($"=== Генерация отчёта: {data} ===");

        // Прогоняем данные через ВСЕ доступные форматы
        foreach (var generator in _generators)
        {
            generator.Generate(data);
        }
    }
}

// ============================================
// ШАГ 4. Новый узел — добавляем без изменения ReportService
// ============================================

// Новый узел — добавляем без изменения существующего кода
public class CsvReport : IReportGenerator
{
    private readonly string _outputPath;

    public CsvReport(string outputPath) => _outputPath = outputPath;

    public void Generate(string data)
    {
        Logger.Info($"[CSV] Генерация отчёта в '{_outputPath}': {data}");
    }
}

// ============================================
// ШАГ 5. Точка входа — сборка сети (композиция)
// ============================================

class Program
{
    static void Main()
    {
        Logger.Info("Запуск системы генерации отчётов");

        // 1. Создаём узлы сети (конкретные реализации)
        var pdf = new PdfReport("report.pdf");
        var excel = new ExcelReport("report.xlsx");
        var html = new HtmlReport("report.html");

        // 2. Формируем сеть: передаём узлы в основной модуль
        var generators = new List<IReportGenerator> { pdf, excel, html };
        var service = new ReportService(generators);

        // 3. Используем систему
        service.GenerateAll("Квартальный отчёт за Q1 2025");

        // 4. Демонстрация расширяемости: добавляем CSV БЕЗ изменения ReportService
        Logger.Info("--- Добавляем новый формат (CSV) ---");
        var csv = new CsvReport("report.csv");
        var newGenerators = new List<IReportGenerator>(generators) { csv };
        var extendedService = new ReportService(newGenerators);

        extendedService.GenerateAll("Квартальный отчёт за Q1 2025");

        Logger.Info("Работа системы завершена");
    }
}