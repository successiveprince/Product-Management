using Microsoft.AspNetCore.Identity;
using Product_Management.Models.Domain;
using System.Data;
using Product_Management.Constant;

namespace Product_Management.Data
{
    public class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            //Seed Roles
            var userManager = service.GetService<UserManager<UserModel>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();


            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));

            // creating new admin

            var user = new UserModel
            {
                UserName= "admin01@gmail.com",
                Name = "Prince Kumar",
                Email = "admin01@gmail.com",              
                UserPassword = "pass@123",
                ConfirmPassword = "pass@123",
                Role="Admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedAt = DateTime.Now
            };
            var userInDb = await userManager.FindByEmailAsync(user.Email);
            if (userInDb == null)
            {
                await userManager.CreateAsync(user, "pass@123");
                await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            }
        }
    }
}
