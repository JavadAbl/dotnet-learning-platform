namespace Contracts.Dto.Request;


public record GetManyQuery(
    int? Page,
    int? PageSize,
    string? SortBy,
    string? SortOrder,
    string? Search
);
