using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Perceval
{
    public class Program
    {
        private const string AdminName = "admin";
        private const string AdminPassword = "admin";

        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

                var admin = await userManager.FindByNameAsync(AdminName);
                if (admin == null)
                {
                    // Create admin account if not exists
                    var adminUser = new IdentityUser(AdminName);
                    await userManager.CreateAsync(adminUser, AdminPassword);
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}