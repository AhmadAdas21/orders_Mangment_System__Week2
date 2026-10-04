using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Week2_Task_2.Data;

namespace Order_Mangment_System_Test
{
    public class CustomWebApplicationFactory
        : WebApplicationFactory<Program>
    {
        private readonly SqliteConnection connection;

        public CustomWebApplicationFactory()
        {
            connection =
                new SqliteConnection("DataSource=:memory:");

            connection.Open();
        }

        protected override void ConfigureWebHost(
            IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<
                    DbContextOptions<data_base>>();

                services.RemoveAll<data_base>();

                services.AddDbContext<data_base>(options =>
                {
                    options.UseSqlite(connection);
                });

                var provider =
                    services.BuildServiceProvider();

                using var scope = provider.CreateScope();

                var db =
                    scope.ServiceProvider
                        .GetRequiredService<data_base>();

                db.Database.EnsureCreated();
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            connection.Close();
            connection.Dispose();
        }
    }
}