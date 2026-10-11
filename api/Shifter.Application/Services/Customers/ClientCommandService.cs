using Shifter.Core.Entities.Customers;
using Shifter.Application.DTOs.Customers;
using Shifter.Application.Interfaces.Repositories.Customers;
using Shifter.Application.Interfaces.Services.Customers;
using Shifter.Application.Interfaces.Repositories.Core;
using Shifter.Application.Interfaces.Services.Locations;
using Shifter.Core.Entities.Common;
using System.Reflection.Metadata.Ecma335;

namespace Shifter.Application.Services.Customers;

public class ClientCommandService(IClientRepository clientRepository, IAddressHandler addressHandler, IUnitOfWork unitOfWork) : IClientCommandService
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IAddressHandler _addressHandler = addressHandler;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    /// <inheritdoc />
    public async Task<ServiceResult<Guid>> CreateAsync(ClientDto clientDto, CancellationToken ct = default)
    {
        var client = Client.Create(
            clientDto.Type, 
            clientDto.CompanyName, 
            clientDto.Abn, 
            clientDto.FirstName, 
            clientDto.LastName, 
            clientDto.Email, 
            clientDto.PhoneNumber
        );

        if (clientDto.Address is not null)
        {
            var address = await _addressHandler.GetOrCreateAddressAsync(clientDto.Address, ct);
            client.SetAddress(address);
        }

        if (clientDto.Contacts is not null)
        {
            foreach (var contactDto in clientDto.Contacts)
            {
                var contact = ClientContact.Create(
                    client.Id,
                    contactDto.FirstName,
                    contactDto.LastName,
                    contactDto.JobTitle,
                    contactDto.Email,
                    contactDto.PhoneNumber
                );
                client.AddContact(contact);
            }
        }

        _clientRepository.Add(client);
        await _unitOfWork.CommitAsync(ct);

        return ServiceResult<Guid>.Success(client.Id);
    }

    /// <inheritdoc />
    public async Task<ServiceResult<bool>> UpdateAsync(Guid guid, ClientDto clientDto, CancellationToken ct = default)
    {
        var client = await _clientRepository.GetByIdAsync(guid, ct);
        if (client == null)
            return ServiceResult<bool>.Failure($"Client not found.");

        client.Update(
            clientDto.Type,
            clientDto.CompanyName,
            clientDto.Abn,
            clientDto.FirstName,
            clientDto.LastName,
            clientDto.Email,
            clientDto.PhoneNumber
        );

        if (clientDto.Address is not null)
        {
            var address = await _addressHandler.GetOrCreateAddressAsync(clientDto.Address, ct);
            client.SetAddress(address);
        }

        await _unitOfWork.CommitAsync(ct);

        return ServiceResult<bool>.Success(true);
    }
}
