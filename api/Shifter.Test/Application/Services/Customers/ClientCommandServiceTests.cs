using FluentAssertions;
using NSubstitute;
using Shifter.Application.DTOs.Customers;
using Shifter.Application.DTOs.Location;
using Shifter.Application.Interfaces.Repositories.Customers;
using Shifter.Application.Interfaces.Repositories.Core;
using Shifter.Application.Interfaces.Services.Locations;
using Shifter.Application.Services.Customers;
using Shifter.Core.Entities.Customers;
using Shifter.Core.Entities.Location;

namespace Shifter.Test.Application.Services.Customers;

public class ClientCommandServiceTests
{
    private readonly IClientRepository _clientRepository;
    private readonly IAddressHandler _addressHandler;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ClientCommandService _sut;

    public ClientCommandServiceTests()
    {
        _clientRepository = Substitute.For<IClientRepository>();
        _addressHandler = Substitute.For<IAddressHandler>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(1);

        _sut = new ClientCommandService(_clientRepository, _addressHandler, _unitOfWork);
    }

    [Fact]
    public async Task CreateAsync_WhenClientIsValid_AddsClientAndReturnsSuccess()
    {
        var addressDto = new AddressDto(
            "place-1",
            "123 Test Street, Brisbane QLD",
            "123",
            "Test Street",
            "Brisbane",
            "4000",
            "QLD",
            "Australia",
            -27.47,
            153.03);

        var contactDto = new ClientContactDto(
            Guid.Empty,
            "Jamie",
            "Jones",
            "Account Manager",
            "jamie@example.com",
            "0400123456");

        var clientDto = new ClientDto(
            Guid.Empty,
            ClientType.Company,
            "Acme Pty Ltd",
            "123456789",
            null,
            null,
            "hello@acme.com",
            "0400111222",
            addressDto,
            [contactDto]);

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
            addressDto.Longitude);

        _addressHandler.GetOrCreateAddressAsync(addressDto, Arg.Any<CancellationToken>()).Returns(address);

        Client? addedClient = null;
        _clientRepository.When(x => x.Add(Arg.Any<Client>())).Do(call => addedClient = call.Arg<Client>());

        var result = await _sut.CreateAsync(clientDto, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);
        addedClient.Should().NotBeNull();
        addedClient!.CompanyName.Should().Be("Acme Pty Ltd");
        addedClient.Address.Should().BeSameAs(address);
        addedClient.Contacts.Should().HaveCount(1);
        addedClient.Contacts.First().FirstName.Should().Be("Jamie");
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_WhenClientIsInvalid_ThrowsArgumentException()
    {
        var clientDto = new ClientDto(
            Guid.Empty,
            ClientType.Company,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null);

        var act = async () => await _sut.CreateAsync(clientDto, CancellationToken.None);

        await act.Should().ThrowAsync<ArgumentException>();
        _clientRepository.DidNotReceive().Add(Arg.Any<Client>());
    }

    [Fact]
    public async Task UpdateAsync_WhenClientExists_UpdatesClientAndContacts()
    {
        var clientId = Guid.NewGuid();
        var existingClient = Client.Create(
            ClientType.Company,
            "Old Co",
            "111111111",
            null,
            null,
            "old@example.com",
            "0400000000");

        existingClient.Id = clientId;
        existingClient.AddContact(ClientContact.Create(clientId, "Old", "Contact", "Sales", "old.contact@example.com", "0400000001"));
        existingClient.AddContact(ClientContact.Create(clientId, "Remove", "Me", "Support", "remove@example.com", "0400000002"));

        var addressDto = new AddressDto(
            "place-2",
            "456 Updated Street, Sydney NSW",
            "456",
            "Updated Street",
            "Sydney",
            "2000",
            "NSW",
            "Australia",
            -33.87,
            151.21);

        var updatedAddress = Address.Create(
            addressDto.PlaceId,
            addressDto.DisplayName,
            addressDto.StreetNumber,
            addressDto.StreetName,
            addressDto.Suburb,
            addressDto.PostCode,
            addressDto.State,
            addressDto.Country,
            addressDto.Latitude,
            addressDto.Longitude);

        var updatedClientDto = new ClientDto(
            clientId,
            ClientType.Company,
            "New Co",
            "222222222",
            null,
            null,
            "new@example.com",
            "0400999999",
            addressDto,
            [
                new ClientContactDto(Guid.Empty, "Updated", "Contact", "Director", "updated@example.com", "0400555666"),
                new ClientContactDto(existingClient.Contacts.First().Id, "Existing", "Contact", "Manager", "existing@example.com", "0400555000")
            ]);

        _clientRepository.GetByIdAsync(clientId, Arg.Any<CancellationToken>()).Returns(existingClient);
        _addressHandler.GetOrCreateAddressAsync(addressDto, Arg.Any<CancellationToken>()).Returns(updatedAddress);

        var result = await _sut.UpdateAsync(clientId, updatedClientDto, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
        existingClient.CompanyName.Should().Be("New Co");
        existingClient.Abn.Should().Be("222222222");
        existingClient.Address.Should().BeSameAs(updatedAddress);
        existingClient.Contacts.Should().HaveCount(2);
        existingClient.Contacts.Should().Contain(contact => contact.FirstName == "Updated" && contact.LastName == "Contact");
        existingClient.Contacts.Should().Contain(contact => contact.Email == "existing@example.com");
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateAsync_WhenClientDoesNotExist_ReturnsFailure()
    {
        var clientId = Guid.NewGuid();
        var clientDto = new ClientDto(
            clientId,
            ClientType.Individual,
            null,
            null,
            "Jane",
            "Doe",
            "jane@example.com",
            null,
            null,
            null);

        _clientRepository.GetByIdAsync(clientId, Arg.Any<CancellationToken>()).Returns((Client?)null);

        var result = await _sut.UpdateAsync(clientId, clientDto, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Client not found.");
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }
}
