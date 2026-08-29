using NSubstitute;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.Employee;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.UnitTests.Application.UseCases.Employee;

public class ChangeEmployeeEmailUseCaseTests {
    private const string ValidCpf = "111.444.777-35";
    private const string ValidEmail = "joao@email.com";
    private const string ValidPhone = "(11) 99999-9999";
    private const string ValidAddress = "Rua das Flores, 123";

    [Fact]
    public async Task ExecuteAsync_WhenEmployeeExists_ChangesEmailAndPersists() {
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        ChangeEmployeeEmailUseCase useCase = new ChangeEmployeeEmailUseCase(repository);

        ProjectOrkestra.Domain.Entities.Employee employee = new ProjectOrkestra.Domain.Entities.Employee(
            Guid.NewGuid(), "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);

        repository.GetByIdAsync(employee.Id).Returns(employee);

        await useCase.ExecuteAsync(employee.Id, "novo@email.com");

        Assert.Equal("novo@email.com", employee.Email);
        await repository.Received(1).UpdateAsync(employee);
    }

    [Fact]
    public async Task ExecuteAsync_WhenEmployeeDoesNotExist_ThrowsNotFoundException() {
        IEmployeeRepository repository = Substitute.For<IEmployeeRepository>();
        ChangeEmployeeEmailUseCase useCase = new ChangeEmployeeEmailUseCase(repository);

        Guid nonExistentId = Guid.NewGuid();
        repository.GetByIdAsync(nonExistentId).Returns((ProjectOrkestra.Domain.Entities.Employee?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => useCase.ExecuteAsync(nonExistentId, "novo@email.com"));
    }
}