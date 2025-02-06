using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pedidos.Infrastructure.Data;

namespace Pedidos.Infrastructure
{
    public static class MigrationExtensions
    {
        public static void EnsureDatabaseMigrated(this IServiceProvider provider)
        {

            var context = provider.GetRequiredService<ApplicationContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

        }

    }
}
