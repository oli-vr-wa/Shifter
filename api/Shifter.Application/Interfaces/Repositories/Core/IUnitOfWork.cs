using Microsoft.EntityFrameworkCore.Storage;

namespace Shifter.Application.Interfaces.Repositories.Core;

public interface IUnitOfWork
{
    /// <summary>
    /// Commits all changes made in the current unit of work to the database.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
    Task<int> CommitAsync();

    /// <summary>
    /// Begins a new database transaction.
    /// </summary>
    /// <returns>The database transaction.</returns>
    Task<IDbContextTransaction> BeginTransactionAsync();
}
