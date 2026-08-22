using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Enums;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.Application.UseCases.Organization;

public class UpdateStatusOrganizationUseCase
{
    private readonly IOrganizationRepository _repository;

    public UpdateStatusOrganizationUseCase(IOrganizationRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(Guid id, OrganizationStatus targetStatus)
    {
        var organization = await _repository.GetByIdAsync(id);

        if (organization is null)
            throw new NotFoundException($"Organization with id '{id}' was not found.");

        if (targetStatus == OrganizationStatus.Active)
            organization.Activate();
        else
            organization.Deactivate();

        await _repository.UpdateAsync(organization);
    }
}
