namespace Shared.Dto.Response;

public record GetManyResponse<T>(
    int TotalCount,
    IEnumerable<T> Items
);
