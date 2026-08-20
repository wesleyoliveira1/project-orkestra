using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Domain.Entities;

namespace ProjectOrkestra.Application.UseCases.BusinessUnit;

public class CreateBusinessUnitUseCase
{
    private readonly IBusinessUnitRepository _repository;

    public CreateBusinessUnitUseCase(
        IBusinessUnitRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> ExecuteAsync(CreateBusinessUnitDto dto){

        var businessUnit = new Domain.Entities.BusinessUnit(dto.OrganizationId, dto.Name, dto.Address);

        await _repository.AddAsync(businessUnit);

        return businessUnit.Id;
    }
}