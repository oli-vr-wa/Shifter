using Microsoft.EntityFrameworkCore;
using Shifter.Application.Interfaces.Repositories.Location;
using Shifter.Core.Entities.Location;
using Shifter.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shifter.Infrastructure.Repositories.Location;

public class AddressRepository(ShifterDbContext dbContext) : IAddressRepository
{
    private readonly ShifterDbContext _dbContext = dbContext;

    /// <inheritdoc />
    public void Add(Address address) => _dbContext.Addresses.Add(address);    

    /// <inheritdoc />
    public async Task<Address?> GetByPlaceIdAsync(string placeId, CancellationToken ct = default) => 
        await _dbContext.Addresses.FirstOrDefaultAsync(a => a.PlaceId == placeId, ct);

    /// <inheritdoc />
    public async Task<Address?> GetByPlaceIdAndStreetNumberAsync(string placeId, string streetNumber, CancellationToken ct = default) => 
        await _dbContext.Addresses.FirstOrDefaultAsync(a => a.PlaceId == placeId && a.StreetNumber == streetNumber, ct);
}
