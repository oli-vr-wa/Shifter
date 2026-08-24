using Shifter.Core.Entities.Tenant;

namespace Shifter.Application.Interfaces.Repositories.CompanyRepos;

public interface ICompanyRepository
{
    /// <summary>
    /// Adds a new company to the database.
    /// This method does not commit the changes to the database; it only adds the company to the context.
    /// </summary>
    /// <param name="company">The company to add.</param>
    Task AddCompanyAsync(Company company);

    /// <summary>
    /// Checks if a company with the specified ABN exists.
    /// </summary>
    /// <param name="abn">The ABN of the company to check.</param>
    /// <returns>True if the company exists; otherwise, false.</returns>
    bool IsCompanyExists(string abn);
}
