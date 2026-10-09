namespace CommerceAI.API.Contracts.Products;

public sealed record UpdateProductRequest(
    string Name,
    decimal Price,
    int Stock,
    uint Version);