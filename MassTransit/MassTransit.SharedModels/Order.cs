namespace MassTransit.SharedModels;
public interface Order
{
    int Id { get; set; }
    string ProductName { get; set; }
    decimal Price { get; set; }
    int Quantity { get; set; }
}
