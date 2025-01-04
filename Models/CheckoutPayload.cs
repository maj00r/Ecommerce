public class OrderDestination
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
}

public class CheckoutPayload : OrderDestination
{
    public string CreditCardNumber { get; set; }
    public string ExpirationDate { get; set; }
    public string Cvv { get; set; }
}
