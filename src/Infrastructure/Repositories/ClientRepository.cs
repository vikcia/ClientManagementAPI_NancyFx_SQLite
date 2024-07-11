using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Dtos;
using System.Data;

namespace Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly IDbConnection _connection;

    public ClientRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<int> CreateAsync(ClientDto clientDto)
    {
        string sql = @"INSERT INTO Clients(
                         Name,
                         Age,
                         Comment)
                     VALUES( 
                         @Name,
                         @Age,
                         @Comment);
                    SELECT last_insert_rowid();";

        var id = await _connection.QuerySingleOrDefaultAsync<int>(sql, clientDto);

        return id;
    }


    public async Task<IEnumerable<ClientEntity>> GetAsync()
    {
        string sql = @"SELECT 
                            Id, 
                            Name, 
                            Age, 
                            Comment 
                        FROM Clients";

        return await _connection.QueryAsync<ClientEntity>(sql);
    }

    public async Task<ClientEntity?> GetByIdAsync(int id)
    {
        string sql = @"SELECT 
                            Name, 
                            Age, 
                            Comment
                        FROM Clients
                        WHERE Id=@Id;";

        return await _connection.QuerySingleOrDefaultAsync<ClientEntity>(sql, new { Id = id });
    }

    public async Task<ClientEntity?> UpdateByIdAsync(int id, ClientDto clientDto)
    {
        string sql = @"UPDATE Clients 
                        SET 
                            Name=@Name,
                            Age=@Age,
                            Comment=@Comment
                        WHERE Id=@Id 
                        RETURNING *";

        var parameters = new { Id = id, Name = clientDto.Name, Age = clientDto.Age, Comment = clientDto.Comment };

        return await _connection.QuerySingleOrDefaultAsync<ClientEntity>(sql, parameters);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        string sql = @"DELETE FROM Clients 
                        WHERE Id=@Id";

        int rowsAffected = await _connection.ExecuteAsync(sql, new { Id = id });

        return rowsAffected > 0;
    }

    public async Task SaveOperationsAsync(int clientId, string status)
    {
        string sql = @"INSERT INTO Operation_history(
                            ClientId,
                            Status,
                            Date)
                        VALUES( 
                            @ClientId,
                            @Status,
                            @Date)";

        var parameters = new { ClientId = clientId, Status = status, Date = DateTime.Now};

        await _connection.ExecuteAsync(sql, parameters);
    }

    public async Task<IEnumerable<OperationsHistoryEntity>> GetHistoryAsync()
    {
        string sql = @"SELECT 
                            ClientId, 
                            Status, 
                            Date
                        FROM Operation_history";

        return await _connection.QueryAsync<OperationsHistoryEntity>(sql);
    }
}