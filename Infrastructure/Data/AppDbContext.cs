using BlockChain.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlockChain.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<BlockchainData> BlockchainData { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlockchainData>(b =>
            {
                //b.ToTable("BlockchainData");
                b.HasKey(x => x.Id);
                b.Property(x => x.BlockchainType).IsRequired();
                b.Property(x => x.RawJson).IsRequired();
                b.Property(x => x.CreatedAt).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
