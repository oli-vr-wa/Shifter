using Microsoft.EntityFrameworkCore.Storage;
using Shifter.Application.Interfaces.Repositories.Core;
using Shifter.Infrastructure.Data;

namespace Shifter.Infrastructure.Repositories.Core;

public class UnitOfWork(ShifterDbContext dbContext) : IUnitOfWork
{
    private readonly ShifterDbContext _dbContext = dbContext;

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task<int> CommitAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}
