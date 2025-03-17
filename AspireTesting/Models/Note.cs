namespace AspireTesting.Models;

public class Note
{
    public int Id { get; set; }

    public string Text { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? RemovedAt { get; set; }
}
