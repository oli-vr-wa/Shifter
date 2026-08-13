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
    public async Task<int> AddOrUpdateAddressAsync(AddressDto addressDto)
    {
        var existingAddress = _addressRepository.GetByPlaceId(addressDto.PlaceId);
        if (existingAddress != null)
        {
            // Update the existing address
            existingAddress.DisplayName = addressDto.DisplayName;
            existingAddress.StreetNumber = addressDto.StreetNumber;
            existingAddress.StreetName = addressDto.StreetName;            
            existingAddress.Suburb = addressDto.Suburb;
            existingAddress.State = addressDto.State;
            existingAddress.PostCode = addressDto.PostCode;
            existingAddress.Country = addressDto.Country;
            existingAddress.Latitude = addressDto.Latitude;
            existingAddress.Longitude = addressDto.Longitude;
            await _unitOfWork.CommitAsync();

            return existingAddress.Id;
        }
        else
        {
            var address = new Address
            {
                PlaceId = addressDto.PlaceId,
                DisplayName = addressDto.DisplayName,
                StreetNumber = addressDto.StreetNumber,
                StreetName = addressDto.StreetName,
                Suburb = addressDto.Suburb,
                State = addressDto.State,
                PostCode = addressDto.PostCode,
                Country = addressDto.Country,
                Latitude = addressDto.Latitude,
                Longitude = addressDto.Longitude
            };
            
            // Add the new address
            await _addressRepository.Add(address);
            await _unitOfWork.CommitAsync();

            return address.Id;
        }
    }
}
