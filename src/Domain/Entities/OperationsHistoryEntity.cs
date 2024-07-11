namespace Domain.Entities;

public class OperationsHistoryEntity
{
    public int ClientId { get; set; }
    public required string Status { get; set; }
    public DateTime Date { get; set; }
}