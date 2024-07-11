namespace DomainModel;

public class ElectricProductDto : ProductDto
{
    public string Voltage { get; set; }

    public string SocketType { get; set; }
}