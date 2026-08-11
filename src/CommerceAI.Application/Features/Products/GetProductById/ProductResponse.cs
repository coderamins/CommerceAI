namespace CommerceAI.Application.Queries.Products.GetProductById;

public record ProductResponse(
    Guid Id,
    string Name,
    decimal Price,
    int Stock);