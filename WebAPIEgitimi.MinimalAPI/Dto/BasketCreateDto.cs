namespace WebAPIEgitimi.MinimalAPI.Dto
{
    public sealed record BasketCreateDto
    (
        Guid ProductId
    );
    public sealed record BasketDto
    (
        Guid Id,
        Guid ProductId,
        string ProductName
    );
}
