using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Enums;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.Application.UseCases.BusinessUnit;

public class UpdateStatusBusinessUnitUseCase
{
    private readonly IBusinessUnitRepository _repository;

    public UpdateStatusBusinessUnitUseCase(IBusinessUnitRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(Guid id, BusinessUnitStatus targetStatus)
    {
        var businessUnit = await _repository.GetByIdAsync(id);

        if (businessUnit is null)
            throw new NotFoundException($"Business Unit with id {id} was not found.");

        if (targetStatus == BusinessUnitStatus.Active)
            businessUnit.Activate();
        else
            businessUnit.Deactivate();

        await _repository.UpdateAsync(businessUnit);
    }
}
