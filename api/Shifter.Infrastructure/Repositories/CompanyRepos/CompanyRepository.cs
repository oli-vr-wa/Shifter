using Shifter.Application.Interfaces.Repositories.CompanyRepos;
using Shifter.Core.Entities.Tenant;
using Shifter.Infrastructure.Data;

namespace Shifter.Infrastructure.Repositories.CompanyRepos;

public class CompanyRepository(ShifterDbContext dbContext) : ICompanyRepository
{
    private readonly ShifterDbContext _dbContext = dbContext;

    /// <inheritdoc/>
    public async Task AddCompanyAsync(Company company)
    {
        if (company == null) throw new ArgumentNullException(nameof(company), "Company cannot be null.");
        if (string.IsNullOrEmpty(company.Abn)) throw new ArgumentOutOfRangeException(nameof(company.Abn), "ABN must be provided");
        if (company.Name == null) throw new ArgumentNullException(nameof(company.Name), "Company name must be provided");

        if (IsCompanyExists(company.Abn))
        {
            throw new InvalidOperationException($"A company with ABN {company.Abn} already exists.");
        }

        await _dbContext.Companies.AddAsync(company);
    }
   
    /// <inheritdoc/>
    public bool IsCompanyExists(string abn)
    {
        return _dbContext.Companies.Any(c => c.Abn == abn);
    }
}
