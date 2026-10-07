using System.ComponentModel.DataAnnotations;

namespace MovieVault.Models;

public class Title
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Зазначте назву твору")]
    [StringLength(200, ErrorMessage = "Назва не може перевищувати 200 символів")]
    public string Name { get; set; } = "";

    [StringLength(200, ErrorMessage = "Оригінальна назва не може перевищувати 200 символів")]
    public string? OriginalName { get; set; }

    public TitleKind Kind { get; set; }

    [Range(1888, 2100, ErrorMessage = "Рік випуску має бути від 1888 до 2100")]
    public int ReleaseYear { get; set; }

    [Range(1, 1000, ErrorMessage = "Тривалість має бути від 1 до 1000 хвилин")]
    public int? DurationMinutes { get; set; }

    [Range(1, 100, ErrorMessage = "Кількість сезонів має бути від 1 до 100")]
    public int? SeasonsCount { get; set; }

    [StringLength(100, ErrorMessage = "Назва країни не може перевищувати 100 символів")]
    public string? Country { get; set; }

    public DateOnly AddedOn { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    public DateOnly? LastWatchedOn { get; set; }
}