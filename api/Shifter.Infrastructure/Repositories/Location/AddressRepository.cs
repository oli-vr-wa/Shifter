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
    public async Task Add(Address address) => await _dbContext.Addresses.AddAsync(address);

    /// <inheritdoc />
    public Address? GetByPlaceId(string placeId) => _dbContext.Addresses.FirstOrDefault(a => a.PlaceId == placeId);
}
