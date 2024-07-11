using Domain.Dtos;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IClientRepository
{
    Task<int> CreateAsync(ClientDto clientDto);
    Task<IEnumerable<ClientEntity>> GetAsync();
    Task<ClientEntity?> GetByIdAsync(int id);
    Task<ClientEntity?> UpdateByIdAsync(int id, ClientDto clientDto);
    Task<bool> DeleteAsync(int id);
    Task SaveOperationsAsync(int clientId, string status);
    Task<IEnumerable<OperationsHistoryEntity>> GetHistoryAsync();
}