using Microsoft.EntityFrameworkCore;
using Shifter.Application.Interfaces.Repositories.Customers;
using Shifter.Core.Entities.Customers;
using Shifter.Infrastructure.Data;

namespace Shifter.Infrastructure.Repositories.Customers;

public class ClientRepository(ShifterDbContext dbContext) : IClientRepository
{
    private readonly ShifterDbContext _dbContext = dbContext;

    /// <inheritdoc />
    public async Task AddAsync(Client client, CancellationToken ct = default)
    {
        await _dbContext.Clients.AddAsync(client, ct);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Client>> GetAllAsync(CancellationToken ct = default)
    {
        return await _dbContext.Clients.ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task<Client?> GetByIdAsync(Guid clientId, CancellationToken ct = default)
    {
        return await _dbContext.Clients
            .Include(c => c.Address)
            .Include(c => c.Contacts)
            .FirstOrDefaultAsync(c => c.Id == clientId, ct);
    }

    /// <inheritdoc />
    public IQueryable<Client> GetQueryable()
    {
        return _dbContext.Clients.AsQueryable();
    }
}
