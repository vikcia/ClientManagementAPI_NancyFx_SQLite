using Domain.Dtos;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IClientService
{
    Task<int> CreateAsync(ClientDto clientDto);
    Task<IEnumerable<ClientEntity>> GetAsync();
    Task<ClientDto> GetByIdAsync(int id);
    Task UpdateByIdAsync(int id, ClientDto clientDto);
    Task DeleteAsync(int id);
    Task<IEnumerable<OperationsHistoryEntity>> GetHistoryAsync();
}