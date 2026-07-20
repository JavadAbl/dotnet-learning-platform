namespace Contracts.Dto.Request;


public record GetManyQuery(
    int Page,
    string PageSize,
    string LastName,
    string SortBy,
    string SortOrder,
    string Search
);
