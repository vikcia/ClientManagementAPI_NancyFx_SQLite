namespace Domain.Dtos;

public class ClientDto
{
    public required string Name { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int Age { get; set;}
}
