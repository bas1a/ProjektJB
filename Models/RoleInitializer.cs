using Microsoft.AspNetCore.Identity;

namespace AccountingProgram.Models
{
    public static class RoleInitializer
    {
        public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager)
        {
            // Lista ról, które mają być zapewnione w systemie
            List<string> roles = new List<string> { "Młodsza Księgowa", "Samodzielna księgowa", "Główna księgowa" };

            foreach (var role in roles)
            {
                // Sprawdź, czy rola już istnieje
                if (!await roleManager.RoleExistsAsync(role))
                {
                    // Jeśli nie istnieje, stwórz nową rolę
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }

}