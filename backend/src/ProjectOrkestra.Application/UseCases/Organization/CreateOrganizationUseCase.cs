using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Exceptions;
using System;

namespace ProjectOrkestra.Application.UseCases.Organization;

public class CreateOrganizationUseCase
{
	private readonly IOrganizationRepository _repository;

	public CreateOrganizationUseCase(
		IOrganizationRepository repository)
	{
        _repository = repository;
	}

    public async Task<Guid> ExecuteAsync(
        CreateOrganizationDto dto)
    {
        var organization = new Domain.Entities.Organization(dto.TenantId, dto.Name, dto.Cnpj);

        await _repository.AddAsync(organization);

        return organization.Id;
    }
}
