using System.Threading.Tasks;
using IntelliTest.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace IntelliTest.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder SeedAdmin(this IApplicationBuilder app)
        {
            string adminRoleName = "Admin";
            string adminEmail = "admin@email.com";
            using var scopedServices = app.ApplicationServices.CreateScope();
            var services = scopedServices.ServiceProvider;
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task.Run(async () =>
                {
                    //If you get exception here: launche the database
                    if (await roleManager.RoleExistsAsync(adminRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = adminRoleName };
                    await roleManager.CreateAsync(role);
                    var admin = await userManager.FindByNameAsync(adminEmail);
                    if (admin is null)
                    {
                        return;
                    }
                    await userManager.AddToRoleAsync(admin, role.Name);
                })
                .GetAwaiter()
                .GetResult();

            return app;
        }

    }
}
