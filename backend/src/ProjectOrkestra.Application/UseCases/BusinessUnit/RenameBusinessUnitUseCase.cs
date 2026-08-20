using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Exceptions;
using ProjectOrkestra.Domain.Entities;
using System;

namespace ProjectOrkestra.Application.UseCases.BusinessUnit;

public class RenameBusinessUnitUseCase
{

    private readonly IBusinessUnitRepository _repository;

    public RenameBusinessUnitUseCase(IBusinessUnitRepository repository){

        _repository = repository;
    }

    public async Task ExecuteAsync(Guid id, string newName){

        var businessUnit = await _repository.GetByIdAsync(id);

        if(businessUnit is null)
            throw new NotFoundException($"Business unit with id {id} was not found.");

        businessUnit.Rename(newName);

        await _repository.UpdateAsync(businessUnit);
    }
}