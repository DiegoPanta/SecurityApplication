using Microsoft.EntityFrameworkCore;
using Presentation.Model.Clientes;
using Presentation.Model.Security;

namespace Presentation.Infrastructure.Database
{
    public class SecurityAppDbContext : DbContext
    {
        public SecurityAppDbContext(DbContextOptions<SecurityAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserConfiguration> UserConfigurations { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfigurationMap());
            modelBuilder.ApplyConfiguration(new ClienteMap());
        }
    }
}
