using Microsoft.EntityFrameworkCore;
using Shifter.Application.Interfaces.Repositories.Customers;
using Shifter.Core.Entities.Customers;
using Shifter.Infrastructure.Data;

namespace Shifter.Infrastructure.Repositories.Customers;

public class ClientContactRepository(ShifterDbContext dbContext) : IClientContactRepository
{
    private readonly ShifterDbContext _dbContext = dbContext;

    /// <inheritdoc />
    public async Task AddAsync(ClientContact clientContact)
    {
        await _dbContext.ClientContacts.AddAsync(clientContact);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ClientContact>> GetAllAsync()
    {
        return await _dbContext.ClientContacts.ToListAsync();
    }

    /// <inheritdoc />
    public async Task<ClientContact?> GetByIdAsync(Guid clientContactId)
    {
        return await _dbContext.ClientContacts.FindAsync(clientContactId);
    }
}
