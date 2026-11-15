using System.Collections.Generic;
using Xunit;

public class ReportServiceTests
{
    /// <summary>
    /// Тестовый двойник узла сети: считает вызовы и запоминает данные.
    /// </summary>
    private sealed class SpyGenerator : IReportGenerator
    {
        public int Calls { get; private set; }
        public string? LastData { get; private set; }

        public void Generate(string data)
        {
            Calls++;
            LastData = data;
        }
    }

    [Fact]
    public void GenerateAll_CallsEveryGeneratorOnce()
    {
        // Arrange
        var pdf = new SpyGenerator();
        var excel = new SpyGenerator();
        var service = new ReportService(new List<IReportGenerator> { pdf, excel });

        // Act
        service.GenerateAll("Квартальный отчёт за Q1 2025");

        // Assert
        Assert.Equal(1, pdf.Calls);
        Assert.Equal(1, excel.Calls);
    }

    [Fact]
    public void GenerateAll_PassesSameDataToAllGenerators()
    {
        // Arrange
        var first = new SpyGenerator();
        var second = new SpyGenerator();
        var service = new ReportService(new List<IReportGenerator> { first, second });
        const string data = "Тестовые данные отчёта";

        // Act
        service.GenerateAll(data);

        // Assert
        Assert.Equal(data, first.LastData);
        Assert.Equal(data, second.LastData);
    }

    [Fact]
    public void GenerateAll_SupportsNewGeneratorWithoutChangingService()
    {
        // Arrange: новый узел подключается только через интерфейс
        var newModule = new SpyGenerator();
        var service = new ReportService(new List<IReportGenerator> { newModule });

        // Act
        service.GenerateAll("Проверка расширяемости");

        // Assert
        Assert.Equal(1, newModule.Calls);
    }
}
