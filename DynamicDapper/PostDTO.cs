namespace DynamicDapper;

public sealed class PostDTO
{
    public int Id { get; init; }

    public string? Body { get; init; }

    public DateTime CreationDate { get; init; }
}
