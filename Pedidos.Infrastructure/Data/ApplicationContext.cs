using Microsoft.EntityFrameworkCore;
using Pedidos.Core.Entities;


namespace Pedidos.Infrastructure.Data
{
    internal class ApplicationContext : DbContext
    {
        public ApplicationContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql($"Host={Environment.GetEnvironmentVariable("DB_HOST")}; Database={Environment.GetEnvironmentVariable("DB_NAME")}; Username={Environment.GetEnvironmentVariable("DB_USER")}; Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};");

            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<ClienteAggregate> Cliente { get; set; }   
        public DbSet<ProdutoAggregate> Produto { get; set; }
        public DbSet<PedidoAggregate> Pedido { get; set; }
        public DbSet<ItemPedido> ItemPedido { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ClienteAggregate>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.OwnsOne(e => e.Email, email =>
                {
                    email.Property(e => e.Endereco).HasColumnName("Email");
                });

                entity.OwnsOne(e => e.CPF, cpf =>
                {
                    cpf.Property(e => e.Value).HasColumnName("CPF");
                });
            });
         
            modelBuilder.Entity<ProdutoAggregate>(Entity =>
            {
                Entity.HasKey(e => e.Id);

                Entity.OwnsOne(e => e.Preco, preco =>
                {
                    preco.Property(e => e.Value).HasColumnName("Preco");
                });
                Entity.HasIndex(e => e.Categoria)
                    .HasDatabaseName("IX_Categoria");
            });

            modelBuilder.Entity<ItemPedido>(Entity => Entity.HasKey(e => e.Id));
            modelBuilder.Entity<PedidoAggregate>(Entity => Entity.HasKey(e => e.Id));
        }
    }
}
