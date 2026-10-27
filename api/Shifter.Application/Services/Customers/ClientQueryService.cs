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
        var safePageNumber = filter.PageNumber <= 0 ? 1 : filter.PageNumber;
        var safePageSize = filter.PageSize <= 0 ? 20 : filter.PageSize;

        var query = _clientRepository.GetQueryable();

        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            query = query.Where(c => (c.CompanyName != null && c.CompanyName.Contains(filter.SearchTerm)) ||
                                     (c.FirstName != null && c.FirstName.Contains(filter.SearchTerm)) ||
                                     (c.LastName != null && c.LastName.Contains(filter.SearchTerm)) ||
                                     (c.Email != null && c.Email.Contains(filter.SearchTerm)) ||
                                     (c.PhoneNumber != null && c.PhoneNumber.Contains(filter.SearchTerm)));
        }

        if (filter.ClientType.HasValue)
        {
            query = query.Where(c => c.Type == filter.ClientType.Value);
        }

        var totalCount = await query.CountAsync(ct);

        // Apply sorting
        query = filter.SortColumn?.ToLower() switch
        {
            "companyname" => filter.SortDescending ? query.OrderByDescending(c => c.CompanyName) : query.OrderBy(c => c.CompanyName),
            "firstname" => filter.SortDescending ? query.OrderByDescending(c => c.FirstName) : query.OrderBy(c => c.FirstName),
            "lastname" => filter.SortDescending ? query.OrderByDescending(c => c.LastName) : query.OrderBy(c => c.LastName),
            _ => query.OrderBy(c => c.CreatedAt) // Default sorting
        };

        var items = await query
            .Skip((safePageNumber - 1) * safePageSize)
            .Take(safePageSize)
            .Select(ClientMappingExtensions.ProjectToListItem)
            .ToListAsync(ct);

        return new PageResult<ClientListItemDto>(items, totalCount, safePageNumber, safePageSize);
    }
}
