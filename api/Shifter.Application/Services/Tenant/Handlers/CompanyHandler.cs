using Shifter.Application.Interfaces.Tenant.Handlers;
using System.Text;

namespace Shifter.Application.Services.Tenant.Handlers;

public class CompanyHandler : ICompanyHandler
{
    /// <inheritdoc/>
    public string FormatAbn(string abn)
    {
        // Remove any non-digit characters
        var digitsOnly = new StringBuilder();
        foreach (var c in abn)
        {
            if (char.IsDigit(c))
            {
                digitsOnly.Append(c);
            }
        }

        if (digitsOnly.Length != 11)
        {
            throw new ArgumentException("ABN must contain exactly 11 digits.");
        }

        return digitsOnly.ToString();
    }
}
