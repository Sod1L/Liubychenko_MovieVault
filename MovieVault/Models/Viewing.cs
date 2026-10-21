using System.ComponentModel.DataAnnotations;

namespace MovieVault.Models;

public class Viewing
{
    public int Id { get; set; }

    // Зовнішній ключ
    
    public int TitleId { get; set; }
    public Title? Title { get; set; }

    public DateOnly WatchedAt { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Range(1, 10, ErrorMessage = "Оцінка має бути від 1 до 10")]
    public int Rating { get; set; }

    public bool IsRewatch { get; set; }

    [StringLength(500, ErrorMessage = "Нотатка не може перевищувати 500 символів")]
    public string? Notes { get; set; }
}