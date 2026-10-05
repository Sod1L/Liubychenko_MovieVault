using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MovieVault.Models;

public static class EnumExtensions
{
    public static string ToUkrainianName(this Enum value)
    {
        var member = value.GetType().GetMember(value.ToString()).FirstOrDefault();
        var attribute = member?.GetCustomAttribute<DisplayAttribute>();
        return attribute?.Name ?? value.ToString();
    }
}