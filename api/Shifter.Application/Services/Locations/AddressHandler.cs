using Shifter.Application.DTOs.Location;
using Shifter.Application.Interfaces.Repositories.Core;
using Shifter.Application.Interfaces.Repositories.Location;
using Shifter.Application.Interfaces.Services.Locations;
using Shifter.Core.Entities.Location;

namespace Shifter.Application.Services.Locations;

public class AddressHandler(
    IAddressRepository addressRepository, 
    IUnitOfWork unitOfWork) : IAddressHandler
{
    private readonly IAddressRepository _addressRepository = addressRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    /// <inheritdoc />
    public async Task<Address> GetOrCreateAddressAsync(AddressDto addressDto, CancellationToken ct = default)
    {
        var existingAddress = await _addressRepository.GetByPlaceIdAndStreetNumberAsync(addressDto.PlaceId, addressDto.StreetNumber, ct);

        if (existingAddress != null)
        {
            return existingAddress;
        }
        else
        {
            var address = Address.Create(
                addressDto.PlaceId,
                addressDto.DisplayName,
                addressDto.StreetNumber,
                addressDto.StreetName,
                addressDto.Suburb,
                addressDto.PostCode,
                addressDto.State,
                addressDto.Country,
                addressDto.Latitude,
                addressDto.Longitude
            );

            // Add the new address
            _addressRepository.Add(address);
            await _unitOfWork.CommitAsync(ct);

            return address;
        }
    }
}
