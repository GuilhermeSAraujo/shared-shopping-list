namespace SL.WebApi.Dtos.Item;

public class CreateItemRequest
{
    public int ListId { get; set; }
    public string Name { get; set; } = string.Empty;
}