using System.Runtime.CompilerServices;
using FluentAssertions;

namespace Shifter.Test;

internal class TestAssemblySetup
{
    /// <summary>
    /// Accepts the FluentAssertions license for the test assembly. This method is called automatically by the .NET runtime before any tests are executed.
    /// This is necessary to avoid the "FluentAssertions license not accepted" warning message when running tests.
    /// </summary>
    [ModuleInitializer]
    internal static void AcceptFluentAssertionsLicense()
    {
        License.Accepted = true;
    }
}
