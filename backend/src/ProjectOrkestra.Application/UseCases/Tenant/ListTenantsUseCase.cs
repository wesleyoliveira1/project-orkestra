using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectOrkestra.Application.DTOs;
using ProjectOrkestra.Application.Interfaces;
using ProjectOrkestra.Domain.Entities;
using ProjectOrkestra.Domain.Exceptions;

namespace ProjectOrkestra.Application.UseCases.Tenant;

public class ListTenantsUseCase
{
    private readonly ITenantRepository _repository;

    public ListTenantsUseCase(ITenantRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Domain.Entities.Tenant>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}
