namespace DomainModel;

public class FreshProductDto : ProductDto
{
    public DateTime ExpiryDate { get; set; }
}