using System;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.Application.UseCases.BusinessUnit;

public class ChangeBusinessUnitAddressUseCase
{
    private readonly IBusinessUnitRepository _repository;

    public ChangeBusinessUnitAddressUseCase(IBusinessUnitRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(Guid id, string newAddress)
    {
        var businessUnit = await _repository.GetByIdAsync(id);

        if (businessUnit is null)
            throw new NotFoundException($"Business unit with id {id} was not found.");

        businessUnit.ChangeAddress(newAddress);

        await _repository.UpdateAsync(businessUnit);
    }
}
