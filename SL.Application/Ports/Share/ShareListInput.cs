namespace SL.Application.Ports.Share;

public class ShareListInput
{
    public int ListId { get; set; }
    public int UserId { get; set; }
    public IEnumerable<string> UsersEmail { get; set; }
}