namespace Domain.Entities;

public class ClientEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int Age { get; set;}
}
