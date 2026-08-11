using CommerceAI.Domain.Common;

namespace CommerceAI.Domain.Entities;

public class Product : BaseEntity
{

    public string Name { get; private set; }

    public decimal Price { get; private set; }

    public int Stock { get; private set; }



    private Product()
    {

    }



    public Product(
        string name,
        decimal price,
        int stock)
    {

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name required");


        if (price <= 0)
            throw new ArgumentException("Price must be positive");


        if (stock < 0)
            throw new ArgumentException("Stock cannot be negative");



        Name = name;
        Price = price;
        Stock = stock;

    }


    public void ChangePrice(decimal price)
    {
        if (price <= 0)
            throw new ArgumentException("Invalid price");


        Price = price;
        Update();
    }


    public void DecreaseStock(int quantity)
    {
        if (quantity > Stock)
            throw new InvalidOperationException(
                "Not enough stock");


        Stock -= quantity;
        Update();
    }

}