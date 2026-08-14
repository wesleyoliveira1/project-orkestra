using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectOrkestra.Application.UseCases.Organization;

public class RenameOrganizationUseCase
{
	private readonly IOrganizationRepository _repository;

	public RenameOrganizationUseCase(
		IOrganizationRepository repository)
	{
        _repository = repository;
	}

    public async Task ExecuteAsync(
        Guid id, string newName)
    {
        var organization = await _repository.GetByIdAsync(id);

        if(organization is null)
            throw new NotFoundException($"Organization with id '{id}' was not found.");

        organization.Rename(newName);

        await _repository.UpdateAsync(organization);
    }
}
