using Microsoft.EntityFrameworkCore;
using MovieVault.Models;

namespace MovieVault.Services; 

public record SummaryRow(string Name, int Count, int TotalMinutes);

public static class SummaryCalculator
{
    // Показник 1: групування за жанром
    public static async Task<List<SummaryRow>> ByGenreAsync(IQueryable<Title> query)
    {
        return await query
            .GroupBy(x => x.Genre!.Name)
            .OrderByDescending(g => g.Count())
            .Select(g => new SummaryRow(
                g.Key, 
                g.Count(), 
                g.Sum(x => x.DurationMinutes ?? 0)
            ))
            .ToListAsync();
    }

    // Показник 2: групування за періодом
    public static async Task<List<SummaryRow>> ByMonthAsync(IQueryable<Title> query)
    {
        // 1. Отримування даних
        var raw = await query
            .GroupBy(x => new { x.AddedOn.Year, x.AddedOn.Month })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                Count = g.Count(),
                Total = g.Sum(x => x.DurationMinutes ?? 0)
            })
            .ToListAsync();

        // 2. Форматування назви періоду
        return raw
            .OrderBy(r => r.Year).ThenBy(r => r.Month)
            .Select(r => new SummaryRow(
                $"{r.Month:D2}.{r.Year}", 
                r.Count, 
                r.Total
            ))
            .ToList();
    }

    // Показник 3: загальний підсумок
    public static async Task<SummaryRow> TotalAsync(IQueryable<Title> query)
    {
        return new SummaryRow(
            "Усього",
            await query.CountAsync(),
            await query.SumAsync(x => x.DurationMinutes ?? 0)
        );
    }
}