using System;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.Application.UseCases.BusinessUnit;

public class GetBusinessUnitUseCase
{
    private readonly IBusinessUnitRepository _repository;

    public GetBusinessUnitUseCase(IBusinessUnitRepository repository)
    {
        _repository = repository;
    }

    public async Task<Domain.Entities.BusinessUnit?> ExecuteAsync(Guid id)
    {
        var businessUnit = await _repository.GetByIdAsync(id);

        if (businessUnit is null)
            throw new NotFoundException($"Business Unit with id {id} was not found");

        return businessUnit;
    }
}
