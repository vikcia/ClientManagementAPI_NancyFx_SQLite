using Application.Services;
using AutoFixture;
using Domain.Interfaces;
using Moq;
using Serilog;
using FluentAssertions;
using FluentValidation.Results;
using Domain.CustomException;
using Domain.Dtos;
using Domain.Entities;

namespace xUnitTests.Services
{
    public class ClientTests
    {
        private readonly Mock<IClientRepository> _clientRepositoryMock;
        private readonly Mock<IClientDtoValidator> _clientDtoValidatorMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly ClientService _clientService;
        private readonly Fixture _fixture;

        public ClientTests()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            _clientDtoValidatorMock = new Mock<IClientDtoValidator>();
            _loggerMock = new Mock<ILogger>();
            _clientService = new ClientService(
                _clientRepositoryMock.Object,
                _loggerMock.Object,
                _clientDtoValidatorMock.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task CreateAsync_Should_Create_Client_When_Valid()
        {
            // Arrange
            var clientDto = _fixture.Create<ClientDto>();
            _clientDtoValidatorMock.Setup(v => v.Validate(clientDto)).Returns(new ValidationResult());
            _clientRepositoryMock.Setup(repo => repo.CreateAsync(clientDto)).ReturnsAsync(1);

            // Act
            var actual = await _clientService.CreateAsync(clientDto);

            // Assert
            actual.Should().Be(1);
            _loggerMock.Verify(l => l.Information("Creating a new client..."), Times.Once);
            _loggerMock.Verify(l => l.Information("Client created successfully => {@ClientDto}", clientDto), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_BadRequestException_When_ValidationFails()
        {
            // Arrange
            var clientDto = _fixture.Create<ClientDto>();
            var validationErrors = new List<ValidationFailure> { new ValidationFailure("Name", "Name is required") };
            _clientDtoValidatorMock.Setup(v => v.Validate(clientDto)).Returns(new ValidationResult(validationErrors));

            // Act
            Func<Task> actual = async () => await _clientService.CreateAsync(clientDto);

            // Assert
            await actual.Should().ThrowAsync<BadRequestException>()
                .WithMessage("Name is required");
        }

        [Fact]
        public async Task GetAsync_Should_Return_Clients_When_Exist()
        {
            // Arrange
            var clients = _fixture.CreateMany<ClientEntity>(3);
            _clientRepositoryMock.Setup(repo => repo.GetAsync()).ReturnsAsync(clients);

            // Act
            var actual = await _clientService.GetAsync();

            // Assert
            actual.Should().BeEquivalentTo(clients);
            _loggerMock.Verify(l => l.Information("Getting clients..."), Times.Once);
        }

        [Fact]
        public async Task GetAsync_Should_Throw_NotFoundException_When_No_Clients()
        {
            // Arrange
            _clientRepositoryMock.Setup(repo => repo.GetAsync()).ReturnsAsync(new List<ClientEntity>());

            // Act
            Func<Task> actual = async () => await _clientService.GetAsync();

            // Assert
            await actual.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Client not found");
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Client_When_Exists()
        {
            // Arrange
            var client = _fixture.Create<ClientEntity>();
            _clientRepositoryMock.Setup(repo => repo.GetByIdAsync(client.Id)).ReturnsAsync(client);

            // Act
            var actual = await _clientService.GetByIdAsync(client.Id);

            // Assert
            actual.Should().BeEquivalentTo(new ClientDto
            {
                Name = client.Name,
                Age = client.Age,
                Comment = client.Comment
            });
        }

        [Fact]
        public async Task GetByIdAsync_Should_Throw_NotFoundException_When_Client_Does_Not_Exist()
        {
            // Arrange
            int id = 1;
            _clientRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((ClientEntity?)null);

            // Act
            Func<Task> actual = async () => await _clientService.GetByIdAsync(id);

            // Assert
            await actual.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"No client found by this id: {id}");
        }

        [Fact]
        public async Task UpdateByIdAsync_Should_Update_Client_When_Valid()
        {
            // Arrange
            var clientDto = _fixture.Create<ClientDto>();
            var clientEntity = _fixture.Create<ClientEntity>();

            int id = 1;
            _clientDtoValidatorMock.Setup(v => v.Validate(clientDto)).Returns(new ValidationResult());
            _clientRepositoryMock.Setup(repo => repo.UpdateByIdAsync(id, clientDto)).ReturnsAsync(clientEntity);

            // Act
            await _clientService.UpdateByIdAsync(id, clientDto);

            // Assert
            _clientRepositoryMock.Verify(repo => repo.UpdateByIdAsync(id, clientDto), Times.Once);
            _loggerMock.Verify(l => l.Information("Client updated successfully => {@clientDto}", clientDto), Times.Once);
        }

        [Fact]
        public async Task UpdateByIdAsync_Should_Throw_BadRequestException_When_ValidationFails()
        {
            // Arrange
            var clientDto = _fixture.Create<ClientDto>();
            var validationErrors = new List<ValidationFailure> { new ValidationFailure("Name", "Name is required") };
            _clientDtoValidatorMock.Setup(v => v.Validate(clientDto)).Returns(new ValidationResult(validationErrors));

            // Act
            Func<Task> actual = async () => await _clientService.UpdateByIdAsync(1, clientDto);

            // Assert
            await actual.Should().ThrowAsync<BadRequestException>()
                .WithMessage("Name is required");
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_Client_When_Exists()
        {
            // Arrange
            var clientEntity = _fixture.Create<ClientEntity>();
            var clientDto = new ClientDto
            {
                Name = clientEntity.Name,
                Age = clientEntity.Age,
                Comment = clientEntity.Comment
            };

            _clientRepositoryMock.Setup(repo => repo.GetByIdAsync(clientEntity.Id)).ReturnsAsync(clientEntity);
            _clientRepositoryMock.Setup(repo => repo.DeleteAsync(clientEntity.Id)).ReturnsAsync(true);

            // Act
            await _clientService.DeleteAsync(clientEntity.Id);

            // Assert
            _clientRepositoryMock.Verify(repo => repo.GetByIdAsync(clientEntity.Id), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.DeleteAsync(clientEntity.Id), Times.Once);
        }


        [Fact]
        public async Task DeleteAsync_Should_Throw_NotFoundException_When_Client_Does_Not_Exist()
        {
            // Arrange
            int id = 1;
            _clientRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((ClientEntity?)null);

            // Act
            Func<Task> actual = async () => await _clientService.DeleteAsync(id);

            // Assert
            await actual.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"No client found to delete with this ID: {id}");
        }

        [Fact]
        public async Task GetHistoryAsync_Should_Return_History_When_Exists()
        {
            // Arrange
            var history = _fixture.CreateMany<OperationsHistoryEntity>(3);
            _clientRepositoryMock.Setup(repo => repo.GetHistoryAsync()).ReturnsAsync(history);

            // Act
            var actual = await _clientService.GetHistoryAsync();

            // Assert
            actual.Should().BeEquivalentTo(history);
            _loggerMock.Verify(l => l.Information("Getting history of operations with clients..."), Times.Once);
            _loggerMock.Verify(l => l.Information("Operations history successfully retrieved => {@history}", history), Times.Once);
        }

        [Fact]
        public async Task GetHistoryAsync_Should_Throw_NotFoundException_When_No_History()
        {
            // Arrange
            _clientRepositoryMock.Setup(repo => repo.GetHistoryAsync()).ReturnsAsync(new List<OperationsHistoryEntity>());

            // Act
            Func<Task> actual = async () => await _clientService.GetHistoryAsync();

            // Assert
            await actual.Should().ThrowAsync<NotFoundException>()
                .WithMessage("No history found");
        }
    }
}