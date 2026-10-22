using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MovieVault.Api;
using MovieVault.Components;
using MovieVault.Data;
using MovieVault.Models;
using MovieVault.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Служби
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddDbContextFactory<MovieVaultContext>(o => o.UseSqlite("Data Source=movievault.db"));
builder.Services.AddQuickGridEntityFrameworkAdapter();
builder.Services.AddOpenApi(); // ЛР 11

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

// Документація (ЛР 11)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// ==========================================
// БЛОК API (МАЄ БУТИ ПЕРЕД MapRazorComponents)
// ==========================================
var api = app.MapGroup("/api");

api.MapGet("/titles", async Task<Ok<List<TitleDto>>> (IDbContextFactory<MovieVaultContext> f) =>
{
    await using var db = await f.CreateDbContextAsync();
    var data = await db.Titles
        .Include(t => t.Genre).Include(t => t.Viewings)
        .Select(t => new TitleDto(
            t.Id, t.Name, t.OriginalName, t.Kind == TitleKind.Film ? "Фільм" : "Серіал",
            t.ReleaseYear, t.Genre!.Name, t.Viewings.Count
        )).ToListAsync();
    return TypedResults.Ok(data);
});

api.MapGet("/titles/{id:int}", async Task<Results<Ok<TitleDto>, NotFound>> (int id, IDbContextFactory<MovieVaultContext> f) =>
{
    await using var db = await f.CreateDbContextAsync();
    var title = await db.Titles
        .Include(t => t.Genre).Include(t => t.Viewings)
        .Where(t => t.Id == id)
        .Select(t => new TitleDto(
            t.Id, t.Name, t.OriginalName, t.Kind == TitleKind.Film ? "Фільм" : "Серіал",
            t.ReleaseYear, t.Genre!.Name, t.Viewings.Count
        )).FirstOrDefaultAsync();

    return title is null ? TypedResults.NotFound() : TypedResults.Ok(title);
});

api.MapGet("/summary", async Task<Ok<List<SummaryRow>>> (IDbContextFactory<MovieVaultContext> f) =>
{
    await using var db = await f.CreateDbContextAsync();
    return TypedResults.Ok(await SummaryCalculator.ByGenreAsync(db.Titles));
});
// ==========================================

// Блок Blazor
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

// Запуск (має бути ТІЛЬКИ ОДИН раз у самому кінці)
app.Run();