namespace Core;

public class ProductValidationException : Exception
{
    public ProductValidationException(string message)
        : base($"Product validation failed due to {message}")
    {
    }
}