using System;
using System.Threading.Tasks;
using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.Application.UseCases.Organization;

public class GetOrganizationUseCase
{
    private readonly IOrganizationRepository _repository;

    public GetOrganizationUseCase(IOrganizationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Domain.Entities.Organization?> ExecuteAsync(Guid id)
    {
        var organization = await _repository.GetByIdAsync(id);

        if (organization is null)
            throw new NotFoundException($"Organization with id '{id}' was not found.");

        return organization;
    }
}
