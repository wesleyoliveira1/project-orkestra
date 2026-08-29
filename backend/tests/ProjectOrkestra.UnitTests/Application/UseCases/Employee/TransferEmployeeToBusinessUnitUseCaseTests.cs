using NSubstitute;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.UseCases.Employee;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.UnitTests.Application.UseCases.Employee;

public class TransferEmployeeToBusinessUnitUseCaseTests {
    private const string ValidCpf = "111.444.777-35";
    private const string ValidEmail = "joao@email.com";
    private const string ValidPhone = "(11) 99999-9999";
    private const string ValidAddress = "Rua das Flores, 123";
    private const string ValidCnpj = "11.222.333/0001-81";

    [Fact]
    public async Task ExecuteAsync_WhenSameOrganization_TransfersAndPersists() {
        IEmployeeRepository employeeRepository = Substitute.For<IEmployeeRepository>();
        IBusinessUnitRepository businessUnitRepository = Substitute.For<IBusinessUnitRepository>();
        var useCase = new TransferEmployeeToBusinessUnitUseCase(employeeRepository, businessUnitRepository);

        var organizationId = Guid.NewGuid();
        var currentBusinessUnit = new ProjectOrkestra.Domain.Entities.BusinessUnit(organizationId, "Loja 1", ValidCnpj, "Rua A, 1");
        var targetBusinessUnit = new ProjectOrkestra.Domain.Entities.BusinessUnit(organizationId, "Loja 2", ValidCnpj, "Rua B, 2");

        var employee = new ProjectOrkestra.Domain.Entities.Employee(
            currentBusinessUnit.Id, "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);

        employeeRepository.GetByIdAsync(employee.Id).Returns(employee);
        businessUnitRepository.GetByIdAsync(currentBusinessUnit.Id).Returns(currentBusinessUnit);
        businessUnitRepository.GetByIdAsync(targetBusinessUnit.Id).Returns(targetBusinessUnit);

        await useCase.ExecuteAsync(employee.Id, targetBusinessUnit.Id);

        Assert.Equal(targetBusinessUnit.Id, employee.BusinessUnitId);
        await employeeRepository.Received(1).UpdateAsync(employee);
    }

    [Fact]
    public async Task ExecuteAsync_WhenEmployeeDoesNotExist_ThrowsNotFoundException() {
        IEmployeeRepository employeeRepository = Substitute.For<IEmployeeRepository>();
        IBusinessUnitRepository businessUnitRepository = Substitute.For<IBusinessUnitRepository>();
        var useCase = new TransferEmployeeToBusinessUnitUseCase(employeeRepository, businessUnitRepository);

        var nonExistentId = Guid.NewGuid();
        employeeRepository.GetByIdAsync(nonExistentId).Returns((ProjectOrkestra.Domain.Entities.Employee?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            useCase.ExecuteAsync(nonExistentId, Guid.NewGuid()));
    }

    [Fact]
    public async Task ExecuteAsync_WhenTargetBusinessUnitDoesNotExist_ThrowsNotFoundException() {
        IEmployeeRepository employeeRepository = Substitute.For<IEmployeeRepository>();
        IBusinessUnitRepository businessUnitRepository = Substitute.For<IBusinessUnitRepository>();
        var useCase = new TransferEmployeeToBusinessUnitUseCase(employeeRepository, businessUnitRepository);

        var organizationId = Guid.NewGuid();
        var currentBusinessUnit = new ProjectOrkestra.Domain.Entities.BusinessUnit(organizationId, "Loja 1", ValidCnpj, "Rua A, 1");
        var employee = new ProjectOrkestra.Domain.Entities.Employee(
            currentBusinessUnit.Id, "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);

        employeeRepository.GetByIdAsync(employee.Id).Returns(employee);
        businessUnitRepository.GetByIdAsync(currentBusinessUnit.Id).Returns(currentBusinessUnit);

        var nonExistentBusinessUnitId = Guid.NewGuid();
        businessUnitRepository.GetByIdAsync(nonExistentBusinessUnitId).Returns((ProjectOrkestra.Domain.Entities.BusinessUnit?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            useCase.ExecuteAsync(employee.Id, nonExistentBusinessUnitId));
    }

    [Fact]
    public async Task ExecuteAsync_WhenTargetBusinessUnitBelongsToDifferentOrganization_ThrowsBusinessRuleException() {
        IEmployeeRepository employeeRepository = Substitute.For<IEmployeeRepository>();
        IBusinessUnitRepository businessUnitRepository = Substitute.For<IBusinessUnitRepository>();
        var useCase = new TransferEmployeeToBusinessUnitUseCase(employeeRepository, businessUnitRepository);

        var currentBusinessUnit = new ProjectOrkestra.Domain.Entities.BusinessUnit(Guid.NewGuid(), "Loja 1", ValidCnpj, "Rua A, 1");
        var targetBusinessUnit = new ProjectOrkestra.Domain.Entities.BusinessUnit(Guid.NewGuid(), "Loja 2", ValidCnpj, "Rua B, 2");

        var employee = new ProjectOrkestra.Domain.Entities.Employee(
            currentBusinessUnit.Id, "João Silva", ValidCpf, ValidEmail, ValidPhone, ValidAddress);

        employeeRepository.GetByIdAsync(employee.Id).Returns(employee);
        businessUnitRepository.GetByIdAsync(currentBusinessUnit.Id).Returns(currentBusinessUnit);
        businessUnitRepository.GetByIdAsync(targetBusinessUnit.Id).Returns(targetBusinessUnit);

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            useCase.ExecuteAsync(employee.Id, targetBusinessUnit.Id));
    }
}