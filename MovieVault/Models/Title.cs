namespace MovieVault.Models;

public class Title
{
    public int Id { get; set; }                    // первинний ключ

    public string Name { get; set; } = "";         // укр. назва
    public string? OriginalName { get; set; }      // оригінальна назва

    public TitleKind Kind { get; set; }            // Film або Series

    public int ReleaseYear { get; set; }           // рік
    public int? DurationMinutes { get; set; }      // тривалість
    public int? SeasonsCount { get; set; }         // кількість сезонів

    public string? Country { get; set; }           // країна виробництва

    public DateOnly AddedOn { get; set; }          // коли додано
    public DateOnly? LastWatchedOn { get; set; }   // коли переглянуто востаннє (може бути відсутня)
}