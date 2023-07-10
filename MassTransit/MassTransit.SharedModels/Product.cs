namespace MassTransit.SharedModels;
public interface Product
{
    Guid Id { get; set; }
    string ProductName { get; set; }
    decimal Price { get; set; }
    int Quantity { get; set; }
}
