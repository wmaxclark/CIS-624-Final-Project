namespace WebPresentation.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebPresentation.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebPresentation.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "WebPresentation.Models.ApplicationDbContext";
        }

        protected override void Seed(WebPresentation.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            const string admin = "test@test.com";
            const string defaultPassword = "password";

            // Prior to breating first user, create roles
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Farmer" });
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Customer" });

            context.SaveChanges(); // Saves the newly added roles

            // Add the admin
            if (!context.Users.Any(u => u.UserName == admin))
            {
                var user = new ApplicationUser()
                {
                    UserName = admin,
                    Email = admin
                };

                IdentityResult result = userManager.Create(user, defaultPassword);


                context.SaveChanges();

                if (result.Succeeded)
                {
                    // Admin created, give admin role
                    userManager.AddToRole(user.Id, "Farmer");
                    context.SaveChanges();
                }
            }
        }
    }
}
