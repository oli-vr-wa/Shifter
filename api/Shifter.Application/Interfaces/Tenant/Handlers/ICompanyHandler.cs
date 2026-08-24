namespace Shifter.Application.Interfaces.Tenant.Handlers;

public interface ICompanyHandler
{
    /// <summary>
    /// Formats the given ABN (Australian Business Number) into a standard format without any spaces or special characters.
    /// </summary>
    /// <param name="abn">The ABN to format.</param>
    /// <returns>The formatted ABN.</returns>
    string FormatAbn(string abn);
}
