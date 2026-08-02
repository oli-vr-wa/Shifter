using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shifter.Core.Entities.Identity;
using Shifter.Core.Entities.Tenant;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shifter.Infrastructure.Data.Seed;

public static class DatabaseSeeder
{
    /// <summary>
    /// Initializes the database with default data, such as creating a test user if it doesn't exist.
    /// This is to be used on development environments only and any test users created should be removed before deploying to production.
    /// </summary>
    /// <param name="serviceProvider">Service provider used to resolve required services.</param>
    /// <returns>The task representing the asynchronous operation.</returns>
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<ShifterDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        // Add a company first
        var companyName = "Test Company";
        var company = await context.Companies.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Name == companyName);

        if (company == null)
        {
            company = new Company( ) { Name = companyName };
            context.Companies.Add(company);
            await context.SaveChangesAsync();
        }

        var testEmail = "test@example.com";
        var userExists = await userManager.FindByEmailAsync(testEmail);

        if (userExists == null)
        {
            var newUser = new User
            {
                UserName = testEmail,
                Email = testEmail,
                EmailConfirmed = true,
                CompanyId = company.Id, // Assign the existing company's ID
            };

            var result = await userManager.CreateAsync(newUser, "SuperSecure1!"); // Use a strong password for the test user

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to seed test user: {errors}");
            }
        }
    }
}
