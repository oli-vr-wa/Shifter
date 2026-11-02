using FluentAssertions;
using MockQueryable;
using NSubstitute;
using Shifter.Application.DTOs.Customers;
using Shifter.Application.Interfaces.Repositories.Customers;
using Shifter.Application.Services.Customers;
using Shifter.Core.Entities.Customers;
using Shifter.Core.Entities.Location;

namespace Shifter.Test.Application.Services.Customers;

public class ClientQueryServiceTests
{
    [Fact]
    public async Task GetByIdAsync_WhenClientExists_ReturnsMappedClientDto()
    {
        var client = CreateIndividualClient("Jane", "Smith", "jane.smith@example.com", "0400123456");
        client.Address = CreateAddress("Test Street", "Brisbane", "QLD");

        var repository = Substitute.For<IClientRepository>();
        repository.GetByIdAsync(client.Id, Arg.Any<CancellationToken>()).Returns(client);
        var sut = new ClientQueryService(repository);

        var result = await sut.GetByIdAsync(client.Id, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(client.Id);
        result.Type.Should().Be(ClientType.Individual);
        result.FirstName.Should().Be("Jane");
        result.LastName.Should().Be("Smith");
        result.Email.Should().Be("jane.smith@example.com");
        result.Address.Should().NotBeNull();
        result.Address!.StreetName.Should().Be("Test Street");
    }

    [Fact]
    public async Task GetByIdAsync_WhenClientDoesNotExist_ReturnsNull()
    {
        var repository = Substitute.For<IClientRepository>();
        repository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Client?)null);
        var sut = new ClientQueryService(repository);

        var result = await sut.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetClientsAsync_WhenFilterAndPaginationAreApplied_ReturnsFilteredPagedResults()
    {
        var client1 = CreateCompanyClient("Acme Ltd", "111111111", "hello@acme.com", "0400111111");
        client1.Address = CreateAddress("Main St", "Sydney", "NSW");

        var client2 = CreateIndividualClient("Bob", "Jones", "bob@example.com", "0400222222");
        var client3 = CreateCompanyClient("Beta Builders", "222222222", "beta@builders.com", "0400333333");

        var repository = Substitute.For<IClientRepository>();
        repository.GetQueryable().Returns(new List<Client> { client1, client2, client3 }.BuildMock());
        var sut = new ClientQueryService(repository);

        var filter = new ClientQueryFilter(
            SearchTerm: "acme",
            PageNumber: 1,
            PageSize: 10,
            SortColumn: "CompanyName",
            SortDescending: false,
            ClientType: ClientType.Company);

        var result = await sut.GetClientsAsync(filter, CancellationToken.None);

        result.Items.Should().HaveCount(1);
        result.TotalCount.Should().Be(1);
        result.Items[0].DisplayName.Should().Be("Acme Ltd");
        result.Items[0].Email.Should().Be("hello@acme.com");
        result.HasNextPage.Should().BeFalse();
        result.HasPreviousPage.Should().BeFalse();
    }

    private static Client CreateIndividualClient(string firstName, string lastName, string? email, string? phoneNumber)
    {
        var client = Client.Create(ClientType.Individual, null, null, firstName, lastName, email, phoneNumber);
        client.Id = Guid.NewGuid();
        return client;
    }

    private static Client CreateCompanyClient(string companyName, string abn, string? email, string? phoneNumber)
    {
        var client = Client.Create(ClientType.Company, companyName, abn, null, null, email, phoneNumber);
        client.Id = Guid.NewGuid();
        return client;
    }

    private static Address CreateAddress(string streetName, string suburb, string state)
    {
        return new Address
        {
            PlaceId = "place-1",
            DisplayName = $"{streetName}, {suburb}, {state}",
            StreetNumber = "123",
            StreetName = streetName,
            Suburb = suburb,
            PostCode = "4000",
            State = state,
            Country = "Australia"
        };
    }
}
