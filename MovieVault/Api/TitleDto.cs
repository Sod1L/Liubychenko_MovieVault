namespace MovieVault.Api;

public record TitleDto(
    int Id,
    string Name,
    string? OriginalName,
    string Kind,
    int ReleaseYear,
    string Genre,
    int ViewingCount
);