# Проект ЛР №1 — система генерации отчётов (версия из main, дополнена из ветки)

![CI](https://github.com/SaLaDuHa1/lab2-vcs/actions/workflows/ci.yml/badge.svg)

## Автор

Гордич Дмитрий, группа ИСТ-41

## Описание

Консольное приложение на C# (.NET 8), демонстрирующее слабую связность
через интерфейсы и внедрение зависимостей.

Центром «сетевой» структуры является интерфейс `IReportGenerator`.
Каждый формат отчёта (`PdfReport`, `ExcelReport`, `HtmlReport`, `CsvReport`,
`MarkdownReport`) реализует этот интерфейс и не зависит от остальных классов.
Основной модуль `ReportService` зависит только от абстракции
`IEnumerable<IReportGenerator>`, поэтому новый формат можно добавить
без изменения существующего кода.

## Архитектура

| Компонент | Роль |
| --- | --- |
| `IReportGenerator` | абстракция узла сети |
| `PdfReport`, `ExcelReport`, `HtmlReport`, `CsvReport`, `MarkdownReport` | конкретные реализации (узлы) |
| `ReportService` | основной модуль, зависит от абстракции |
| `Program` | композиция сети и точка входа |

## Установка

Требуется [.NET SDK 8.0](https://dotnet.microsoft.com/download) или новее.

```bash
git clone https://github.com/SaLaDuHa1/lab2-vcs.git
cd lab2-vcs
dotnet restore
```

## Запуск

```bash
dotnet run --project Lab1Kalk
```

## Логирование

Модуль `Logger` (`Lab1Kalk/Logger.cs`) пишет все события приложения
в консоль и в файл `logs/app.log` (каталог создаётся автоматически).
Уровни логирования: `INFO`, `WARN`, `ERROR`.

## Тестирование

Модульные тесты (xUnit) проверяют, что `ReportService` вызывает все
подключённые генераторы и передаёт им одни и те же данные.

```bash
dotnet test
```

CI-пайплайн GitHub Actions автоматически собирает проект и запускает
тесты при каждом push и pull request в ветку `main`
(`.github/workflows/ci.yml`).

## Структура проекта

```text
Lab1Kalk.sln
├── Lab1Kalk/
│   ├── Lab1Kalk.csproj
│   ├── Logger.cs
│   ├── MarkdownReport.cs
│   └── Program.cs
├── Lab1Kalk.Tests/
│   ├── Lab1Kalk.Tests.csproj
│   └── ReportServiceTests.cs
└── .github/workflows/ci.yml
```
