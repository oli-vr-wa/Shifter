using Microsoft.EntityFrameworkCore;
using Shifter.Application.DTOs.Common;
using Shifter.Application.DTOs.Customers;
using Shifter.Application.Interfaces.Repositories.Customers;
using Shifter.Application.Interfaces.Services.Customers;
using Shifter.Application.Mappings.Customers;

namespace Shifter.Application.Services.Customers;

public class ClientQueryService(IClientRepository clientRepository) : IClientQueryService
{
    private readonly IClientRepository _clientRepository = clientRepository;

    /// <inheritdoc />
    public async Task<ClientDto?> GetByIdAsync(Guid clientId, CancellationToken ct = default)
    {
        var client = await _clientRepository.GetByIdAsync(clientId, ct);

        if (client is null)
        {
            return null;
        }

        var clientDto = client.ToDto();

        return clientDto;
    }

    /// <inheritdoc />
    public async Task<PageResult<ClientListItemDto>> GetClientsAsync(ClientQueryFilter filter, CancellationToken ct = default)
    {
        var query = _clientRepository.GetQueryable();

        if (filter.SearchTerm is { } term)
        {
            query = query.Where(c => (c.CompanyName != null && c.CompanyName.ToLower().Contains(term)) ||
                                     (c.FirstName != null && c.FirstName.ToLower().Contains(term)) ||
                                     (c.LastName != null && c.LastName.ToLower().Contains(term)) ||
                                     (c.Email != null && c.Email.ToLower().Contains(term)) ||
                                     (c.PhoneNumber != null && c.PhoneNumber.Contains(term)));
        }

        if (filter.ClientType.HasValue)
        {
            query = query.Where(c => c.Type == filter.ClientType.Value);
        }

        var totalCount = await query.CountAsync(ct);

        // Apply sorting
        query = filter.SortColumn?.ToLower() switch
        {
            "companyname" => filter.SortDescending 
                ? query.OrderByDescending(c => c.CompanyName).ThenByDescending(c => c.Id)
                : query.OrderBy(c => c.CompanyName).ThenBy(c => c.Id),
            "firstname" => filter.SortDescending 
                ? query.OrderByDescending(c => c.FirstName).ThenByDescending(c => c.Id) 
                : query.OrderBy(c => c.FirstName).ThenBy(c => c.Id),
            "lastname" => filter.SortDescending 
                ? query.OrderByDescending(c => c.LastName).ThenByDescending(c => c.Id) 
                : query.OrderBy(c => c.LastName).ThenBy(c => c.Id),
            _ => filter.SortDescending // Default sorting
                ? query.OrderByDescending(c => c.CreatedAt).ThenByDescending(c => c.Id) 
                : query.OrderBy(c => c.CreatedAt).ThenBy(c => c.Id) 
        };

        var items = await query
            .Skip(filter.Skip)
            .Take(filter.PageSize)
            .Select(ClientMappingExtensions.ProjectToListItem)
            .ToListAsync(ct);

        return new PageResult<ClientListItemDto>(items, totalCount, filter.PageNumber, filter.PageSize);
    }
}
