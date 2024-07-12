using Application.Validators;
using Domain.Dtos;
using FluentValidation.TestHelper;
using AutoFixture;

namespace xUnitTests.Validators
{
    public class ClientDtoValidatorTests
    {
        private readonly ClientDtoValidator _validator;
        private readonly Fixture _fixture;

        public ClientDtoValidatorTests()
        {
            _validator = new ClientDtoValidator();
            _fixture = new Fixture();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Age_Should_Not_Be_Invalid(int age)
        {
            // Arrange
            var clientDto = _fixture.Build<ClientDto>()
                .With(c => c.Name, "John")
                .With(c => c.Age, age)
                .With(c => c.Comment, "This is a comment.")
                .Create();

            // Act
            var actual = _validator.TestValidate(clientDto);

            // Assert
            actual.ShouldHaveValidationErrorFor(x => x.Age);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Name_Should_Not_Be_Null_Or_Empty(string name)
        {
            // Arrange
            var clientDto = _fixture.Build<ClientDto>()
                .With(c => c.Name, name)
                .With(c => c.Age, 25)
                .With(c => c.Comment, "This is a comment.")
                .Create();

            // Act
            var actual = _validator.TestValidate(clientDto);

            // Assert
            actual.ShouldHaveValidationErrorFor(x => x.Name);
        }

        

        [Fact]
        public void ClientDto_Should_Be_Valid()
        {
            // Arrange
            var clientDto = _fixture.Build<ClientDto>()
                .With(c => c.Name, "John")
                .With(c => c.Age, 25)
                .With(c => c.Comment, "This is a valid comment.")
                .Create();

            // Act
            var actual = _validator.TestValidate(clientDto);

            // Assert
            actual.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("12345")]
        public void Comment_Should_Validate(string comment)
        {
            // Arrange
            var clientDto = _fixture.Build<ClientDto>()
                .With(c => c.Name, "John")
                .With(c => c.Age, 25)
                .With(c => c.Comment, comment)
                .Create();

            // Act
            var actual = _validator.TestValidate(clientDto);

            // Assert
            actual.ShouldHaveValidationErrorFor(x => x.Comment);
        }
    }
}