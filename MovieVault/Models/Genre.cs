using System.ComponentModel.DataAnnotations;

namespace MovieVault.Models;

public class Genre
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Зазначте назву жанру")]
    [StringLength(50, ErrorMessage = "Назва жанру не може перевищувати 50 символів")]
    public string Name { get; set; } = "";

    [StringLength(200, ErrorMessage = "Опис не може перевищувати 200 символів")]
    public string? Description { get; set; }
}