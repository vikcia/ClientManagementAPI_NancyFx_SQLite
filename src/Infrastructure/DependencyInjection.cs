using Application.Validators;
using Domain.Interfaces;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        //SQLite connection
        var connectionString = configuration["DatabaseName"] ?? throw new ArgumentNullException("SQLite connection string was not found.");
        services.AddTransient<IDbConnection>(sp => new SqliteConnection(connectionString));

        services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();
        services.AddTransient<IClientRepository, ClientRepository>();

        //Validator registration
        services.AddTransient<IClientDtoValidator, ClientDtoValidator>();
    }
}