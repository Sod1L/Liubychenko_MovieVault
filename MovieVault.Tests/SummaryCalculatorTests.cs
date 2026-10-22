using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MovieVault.Data;
using MovieVault.Models;
using MovieVault.Services;

namespace MovieVault.Tests;

public class SummaryCalculatorTests : IDisposable
{
    private readonly SqliteConnection connection;
    private readonly MovieVaultContext db;

    public SummaryCalculatorTests()
    {
        // Створюємо базу даних SQLite в оперативній пам'яті
        connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<MovieVaultContext>()
            .UseSqlite(connection)
            .Options;

        db = new MovieVaultContext(options);
        db.Database.EnsureCreated(); // Створює таблиці
    }

    public void Dispose()
    {
        // Звільняємо ресурси після кожного тесту
        db.Dispose();
        connection.Dispose();
    }

    // 1. Межовий випадок: порожня база
    [Fact]
    public async Task ByGenreForEmptySetReturnsNoRows()
    {
        var result = await SummaryCalculator.ByGenreAsync(db.Titles);
        Assert.Empty(result);
    }

    // 2. Кілька записів з однаковим жанром зливаються в один рядок
    [Fact]
    public async Task TitlesOfOneGenreMergeIntoSingleRow()
    {
        var genre = new Genre { Name = "Фантастика" };
        db.Genres.Add(genre);
        await db.SaveChangesAsync();

        db.Titles.AddRange(
            new Title { Name = "Дюна", ReleaseYear = 2021, Kind = TitleKind.Film, DurationMinutes = 155, GenreId = genre.Id },
            new Title { Name = "Інтерстеллар", ReleaseYear = 2014, Kind = TitleKind.Film, DurationMinutes = 169, GenreId = genre.Id }
        );
        await db.SaveChangesAsync();

        var result = await SummaryCalculator.ByGenreAsync(db.Titles);
        
        var row = Assert.Single(result);
        Assert.Equal("Фантастика", row.Name);
        Assert.Equal(2, row.Count);
        Assert.Equal(324, row.TotalMinutes); // 155 + 169
    }

    // 3. Загальний підсумок дорівнює сумі значень
    [Fact]
    public async Task TotalReturnsSumOfDurations()
    {
        var genre = new Genre { Name = "Драма" };
        db.Genres.Add(genre);
        await db.SaveChangesAsync();

        db.Titles.AddRange(
            new Title { Name = "Твір 1", ReleaseYear = 2020, Kind = TitleKind.Film, DurationMinutes = 100, GenreId = genre.Id },
            new Title { Name = "Твір 2", ReleaseYear = 2021, Kind = TitleKind.Film, DurationMinutes = 120, GenreId = genre.Id }
        );
        await db.SaveChangesAsync();

        var result = await SummaryCalculator.TotalAsync(db.Titles);
        
        Assert.Equal("Усього", result.Name);
        Assert.Equal(2, result.Count);
        Assert.Equal(220, result.TotalMinutes);
    }

    // 4. Записи різних періодів (місяців) потрапляють у різні рядки
    [Fact]
    public async Task ByMonthForDifferentPeriodsReturnsSeparateRows()
    {
        var genre = new Genre { Name = "Комедія" };
        db.Genres.Add(genre);
        await db.SaveChangesAsync();

        db.Titles.AddRange(
            new Title { Name = "Січневий", ReleaseYear = 2020, Kind = TitleKind.Film, DurationMinutes = 90, AddedOn = new DateOnly(2026, 1, 15), GenreId = genre.Id },
            new Title { Name = "Лютневий", ReleaseYear = 2021, Kind = TitleKind.Film, DurationMinutes = 90, AddedOn = new DateOnly(2026, 2, 10), GenreId = genre.Id }
        );
        await db.SaveChangesAsync();

        var result = await SummaryCalculator.ByMonthAsync(db.Titles);
        
        Assert.Equal(2, result.Count);
        Assert.Equal("01.2026", result[0].Name);
        Assert.Equal("02.2026", result[1].Name);
    }

    // 5. Один запис дає рівно один рядок із кількістю 1
    [Fact]
    public async Task SingleTitleReturnsOneRowWithCountOne()
    {
        var genre = new Genre { Name = "Жахи" };
        db.Genres.Add(genre);
        await db.SaveChangesAsync();

        db.Titles.Add(new Title { Name = "Один", ReleaseYear = 2025, Kind = TitleKind.Film, DurationMinutes = 105, GenreId = genre.Id });
        await db.SaveChangesAsync();

        var result = await SummaryCalculator.ByGenreAsync(db.Titles);
        
        var row = Assert.Single(result);
        Assert.Equal(1, row.Count);
        Assert.Equal(105, row.TotalMinutes);
    }
}