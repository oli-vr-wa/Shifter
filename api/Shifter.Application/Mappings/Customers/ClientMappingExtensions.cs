using Shifter.Application.DTOs.Customers;
using Shifter.Application.Mappings.Location;
using Shifter.Core.Entities.Customers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Shifter.Application.Mappings.Customers;

public static class ClientMappingExtensions
{
    /// <summary>
    /// Maps a Client entity to a ClientDto.
    /// </summary>
    /// <param name="client">The Client entity to map.</param>
    /// <returns>A ClientDto representing the mapped Client entity.</returns>
    public static ClientDto ToDto(this Client client) => new(
        client.Id,
        client.Type,
        client.CompanyName,
        client.Abn,
        client.FirstName,
        client.LastName,
        client.Email,
        client.PhoneNumber,
        client.Address?.ToDto(),
        client.Contacts?.Select(c => c.ToDto()).ToList()
    );

    /// <summary>
    /// Maps a ClientContact entity to a ClientContactDto.
    /// </summary>
    /// <param name="contact">The ClientContact entity to map.</param>
    /// <returns>A ClientContactDto representing the mapped ClientContact entity.</returns>
    public static ClientContactDto ToDto(this ClientContact contact) => new(
        contact.Id,
        contact.FirstName,
        contact.LastName,
        contact.JobTitle,
        contact.Email,
        contact.PhoneNumber
    );

    /// <summary>
    /// Maps a Client entity to a ClientListItemDto.
    /// </summary>
    /// <param name="client">The Client entity to map.</param>
    /// <returns>A ClientListItemDto representing the mapped Client entity.</returns>
    public static ClientListItemDto ToListItemDto(this Client client) => new(
        client.Id,
        client.Type,
        client.DisplayName,
        client.Email,
        client.PhoneNumber,
        client!.Address != null ? client.Address.ShortAddress : null
    );

    /// <summary>
    /// Provides an expression to project a Client entity to a ClientListItemDto.
    /// </summary>
    public static Expression<Func<Client, ClientListItemDto>> ProjectToListItem
    {
        get
        {
            return c => new ClientListItemDto(
                c.Id,
                c.Type,
                c.DisplayName,
                c.Email,
                c.PhoneNumber,
                c.Address != null ? $"{c.Address.StreetNumber} {c.Address.StreetName}, {c.Address.Suburb}, {c.Address.State} {c.Address.PostCode}" : null
            );
        }
    }
}
