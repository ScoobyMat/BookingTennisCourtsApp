using BookingTennisCourts;
using Microsoft.AspNetCore.Identity;


public static class IdentityInitializer
{
    public static async Task InitializeRolesAndUsersAsync(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            try
            {
                var roles = new[] { "Admin", "User" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                        Console.WriteLine($"Rola {role} została pomyślnie utworzona.");
                    }
                    else
                    {
                        Console.WriteLine($"Rola {role} już istnieje.");
                    }
                }

                string email = "admin@wp.pl";
                string password = "Admin123$";
                string FirstName = "Admin";
                string LastName = "Admin";
                DateTime DateOfBirth = new DateTime(2000, 1, 1);

                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new User
                    {
                        UserName = email,
                        Email = email,
                        FirstName = FirstName,
                        LastName = LastName,
                        DateOfBirth = DateOfBirth
                    };

                    var result = await userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                        Console.WriteLine("Użytkownik Admin został pomyślnie utworzony.");
                    }
                    else
                    {
                        Console.WriteLine("Błąd przy tworzeniu użytkownika:");
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine(error.Description);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Użytkownik Admin już istnieje.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd: {ex.Message}");
            }
        }
    }
}
