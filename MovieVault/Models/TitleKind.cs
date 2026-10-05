using System.ComponentModel.DataAnnotations;

namespace MovieVault.Models;

public enum TitleKind
{
    [Display(Name = "Фільм")]
    Film,

    [Display(Name = "Серіал")]
    Series
}