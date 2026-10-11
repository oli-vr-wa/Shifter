using Shifter.Core.Entities.Customers;
using Shifter.Application.DTOs.Customers;
using Shifter.Application.Interfaces.Repositories.Customers;
using Shifter.Application.Interfaces.Services.Customers;
using Shifter.Application.Interfaces.Repositories.Core;
using Shifter.Application.Interfaces.Services.Locations;

namespace Shifter.Application.Services.Customers;

public class ClientCommandService(IClientRepository clientRepository, IAddressHandler addressHandler, IUnitOfWork unitOfWork) : IClientCommandService
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IAddressHandler _addressHandler = addressHandler;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    /// <inheritdoc />
    public async Task<Guid> CreateAsync(ClientDto clientDto, CancellationToken ct = default)
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

        return client.Id;
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Guid guid, ClientDto clientDto, CancellationToken ct = default)
    {
        var client = await _clientRepository.GetByIdAsync(guid, ct) ?? throw new KeyNotFoundException($"Client record not found.");

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
    }
}
