using Domain.Entities;
using Domain.Interfaces;
using Domain.CustomException;
using Domain.Dtos;
using Serilog;

namespace Application.Services;

public enum Status
{
    ClientCreated,
    ClientEdited,
    ClientDeleted
}

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;
    private readonly ILogger _logger;
    private readonly IClientDtoValidator _clientValidator;

    public ClientService(IClientRepository clientRepository, ILogger logger, IClientDtoValidator clientValidator)
    {
        _clientRepository = clientRepository;
        _logger = logger;
        _clientValidator = clientValidator;
    }

    public async Task<int> CreateAsync(ClientDto clientDto)
    {
        var result = _clientValidator.Validate(clientDto);

        if (!result.IsValid)
        {
            var validationErrors = string.Join(", ", result.Errors.Select(error => error.ErrorMessage));
            throw new BadRequestException(validationErrors);
        }

        try
        {
            _logger.Information("Creating a new client...");

            int clientId = await _clientRepository.CreateAsync(clientDto);

            await _clientRepository.SaveOperationsAsync(clientId, Status.ClientCreated.ToString());

            _logger.Information("Client created successfully => {@ClientDto}", clientDto);

            return clientId;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while creating a client");
            throw;
        }
    }

    public async Task<IEnumerable<ClientEntity>> GetAsync()
    {
        try
        {
            _logger.Information("Getting clients...");

            IEnumerable<ClientEntity> clients = await _clientRepository.GetAsync();

            _ = (clients == null || !clients.Any())
               ? throw new NotFoundException($"Client not found") : clients;

            _logger.Information("Clients retrieved successfully => {@clients}", clients);

            return clients;
        }
        catch (NotFoundException ex)
        {
            _logger.Warning(ex, "Client not found");
            throw;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while getting clients");
            throw;
        }
    }

    public async Task<ClientDto> GetByIdAsync(int id)
    {
        if (id.GetType() != typeof(int))
            throw new ArgumentException("ID must be an integer");

        try
        {
            _logger.Information("Getting client...");

            ClientEntity client = await _clientRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"No client found by this id: {id}");

            _logger.Information("Client retrieved successfully => {@client}", client);

            return new()
            {
                Name = client.Name,
                Age = client.Age,
                Comment = client.Comment
            };
        }
        catch (NotFoundException ex)
        {
            _logger.Warning(ex, "Client with ID: {Id} was not found", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while getting client by Id");
            throw;
        }
    }

    public async Task UpdateByIdAsync(int id, ClientDto clientDto)
    {
        if (id.GetType() != typeof(int))
            throw new ArgumentException("ID must be an integer");

        var result = _clientValidator.Validate(clientDto);

        if (!result.IsValid)
        {
            var validationErrors = string.Join(", ", result.Errors.Select(error => error.ErrorMessage));
            throw new BadRequestException(validationErrors);
        }

        try
        {
            _logger.Information("Updating client...");

            _ = await _clientRepository.UpdateByIdAsync(id, clientDto)
                ?? throw new NotFoundException($"No client by this ID: {id}");

            await _clientRepository.SaveOperationsAsync(id, Status.ClientEdited.ToString());

            _logger.Information("Client updated successfully => {@clientDto}", clientDto);
        }
        catch (NotFoundException ex)
        {
            _logger.Warning(ex, "Client with ID: {Id} was not found", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while getting clients");
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        if (id.GetType() != typeof(int))
            throw new ArgumentException("ID must be an integer");

        try
        {
            _logger.Information("Deleting client with ID: {Id}", id);

            ClientEntity clientEntity = await _clientRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"No client found to delete with this ID: {id}");

            ClientDto clientDto = new ClientDto
            {
                Name = clientEntity.Name,
                Age = clientEntity.Age,
                Comment = clientEntity.Comment
            };

            bool isDeleted = await _clientRepository.DeleteAsync(id);
            if (!isDeleted)
            {
                throw new NotFoundException($"No client found to delete with this ID: {id}");
            }

            await _clientRepository.SaveOperationsAsync(id, Status.ClientDeleted.ToString());

            _logger.Information("Client deleted successfully => {@clientDto}", clientDto);
        }
        catch (NotFoundException ex)
        {
            _logger.Warning(ex, "Client with ID: {Id} was not found to delete", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while deleting client with ID: {Id}", id);
            throw;
        }
    }


    public async Task<IEnumerable<OperationsHistoryEntity>> GetHistoryAsync()
    {
        try
        {
            _logger.Information("Getting history of operations with clients...");

            IEnumerable<OperationsHistoryEntity> history = await _clientRepository.GetHistoryAsync();

            _ = (history == null || !history.Any())
                ? throw new NotFoundException($"No history found") : history;

            _logger.Information("Operations history successfully retrieved => {@history}", history);

            return history;
        }
        catch (NotFoundException ex)
        {
            _logger.Warning(ex, "No history found");
            throw;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while getting history");
            throw;
        }
    }
}