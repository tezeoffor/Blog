using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlogApplication.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlogApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            try
            {



                var scope = host.Services.CreateScope();

                var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>(); //handles all the user account
                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>(); //handles all the role you can assign to users

                ctx.Database.EnsureCreated();

                //checks if thers any roles assinged
                var adminRole = new IdentityRole("Admin");
                if (!ctx.Roles.Any())
                {
                    roleMgr.CreateAsync(adminRole).GetAwaiter().GetResult(); //create a role
                }

                //if admin doesnt exist create a admin
                if (!ctx.Users.Any(u => u.UserName == "member1"))
                {
                    //create an admin
                    var adminUser = new IdentityUser
                    {
                        UserName = "member1",
                        Email = "member1@email.com"
                    };
                    var result = userMgr.CreateAsync(adminUser, "password123").GetAwaiter().GetResult();

                    //add role to user
                    userMgr.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
