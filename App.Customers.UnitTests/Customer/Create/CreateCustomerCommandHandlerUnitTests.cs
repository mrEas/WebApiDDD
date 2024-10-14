using App.Application.Customers.Commands;
using App.Domain.Customers;
using App.Domain.DomainErrors;
using App.Domain.Primitives;
using App.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using System.Numerics;
namespace App.UnitTests.Customer.Create
{
    public class CreateCustomerCommandHandlerUnitTests
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly CreateCustomerCommandHandler _handler;

        public CreateCustomerCommandHandlerUnitTests()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _handler = new CreateCustomerCommandHandler(_mockUnitOfWork.Object, _mockCustomerRepository.Object);
        }

        [Theory]
        [InlineData("888-123456")]
        [InlineData("123-456-7890")]
        [InlineData("80512345")]
        public async Task HandleCreateCustomer_WhenPhoneNumberHasBadFormat_ShouldReturnPhoneNumberIsNotValid(string phone)
        {
            //Arrange
            CreateCustomerCommand command = new CreateCustomerCommand(
                "John", "Doe",
                "john.doe@example.com",
                 phone,
                "USA", "Line 1", "Line 2", "City", "State", "12345");

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorOr.ErrorType.Validation);
            result.FirstError.Code.Should().Be(App.Domain.DomainErrors.CustomerErrors.PhoneNumberIsNotValid.Code);
            result.FirstError.Description.Should().Be(App.Domain.DomainErrors.CustomerErrors.PhoneNumberIsNotValid.Description);
        }

        [Fact]
        public async Task HandleCreateCustomer_WhenEmailHasBadFormat_ShouldReturnEmailIsNotValid()
        {
            //Arrange
            CreateCustomerCommand command = new CreateCustomerCommand(
              "John", "Doe",
              "john.doedsd.com",
              "888-88888",
              "USA", "Line 1", "Line 2", "City", "State", "12345");

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorOr.ErrorType.Validation);
            result.FirstError.Code.Should().Be(App.Domain.DomainErrors.CustomerErrors.EmailIsNotValid.Code);
            result.FirstError.Description.Should().Be(App.Domain.DomainErrors.CustomerErrors.EmailIsNotValid.Description);
        }

        [Fact]
        public async Task Handle_WhenEmailAlreadyExists_ShouldReturnEmailAlreadyExistsError()
        {
            //Arrange
            var command = new CreateCustomerCommand(
                "John", "Doe",
                "john.doe@example.com",
                "888-88888",
                "USA", "Line 1", "Line 2", "City", "State", "12345");

            _mockCustomerRepository.Setup(repo => repo.IsExistsByEmailAsync(It.IsAny<Email>()))
                .ReturnsAsync(true);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorOr.ErrorType.Validation);
            result.FirstError.Code.Should().Be(App.Domain.DomainErrors.CustomerErrors.EmailAlreadyExists.Code);
            result.FirstError.Description.Should().Be(App.Domain.DomainErrors.CustomerErrors.EmailAlreadyExists.Description);
        }

        [Fact]
        public async Task Handle_WhenPhoneAlreadyExists_ShouldReturnPhoneAlreadyExistsError()
        {
            //Arrange
            var command = new CreateCustomerCommand(
                "John", "Doe",
                "john.doe@example.com",
                "888-88888",
                "USA", "Line 1", "Line 2", "City", "State", "12345");

            var phoneNumber = PhoneNumber.Create(command.PhoneNumber);

            _mockCustomerRepository.Setup(repo => repo.IsExistsByPhoneAsync(phoneNumber))
                .ReturnsAsync(true);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorOr.ErrorType.Validation);
            result.FirstError.Code.Should().Be(App.Domain.DomainErrors.CustomerErrors.PhoneAlreadyExists.Code);
            result.FirstError.Description.Should().Be(App.Domain.DomainErrors.CustomerErrors.PhoneAlreadyExists.Description);
        }

        [Fact]
        public async Task Handle_WhenCustomerIsValid_ShouldAddCustomerAndCommit()
        {
            //Arrange
            var command = new CreateCustomerCommand(
                "John", "Doe",
                "john.doe@example.com",
                "888-88888",
                "USA", "Line 1", "Line 2", "City", "State", "12345");

            var email = Email.Create(command.Email);
            var phoneNumber = PhoneNumber.Create(command.PhoneNumber);
            var address = Address.Create(command.Country, command.Line1, command.Line2, command.City, command.State, command.ZipCode);

            _mockCustomerRepository.Setup(repo => repo.IsExistsByEmailAsync(email))
                .ReturnsAsync(false);

            _mockCustomerRepository.Setup(repo => repo.IsExistsByPhoneAsync(phoneNumber))
                .ReturnsAsync(false);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeFalse();

            _mockCustomerRepository.Verify(repo => repo.Add(It.IsAny<App.Domain.Customers.Customer>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }

}
