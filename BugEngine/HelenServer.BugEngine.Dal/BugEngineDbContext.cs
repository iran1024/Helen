using HelenServer.Data.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace HelenServer.BugEngine.Dal
{
    public partial class BugEngineDbContext : DbContext
    {
        public BugEngineDbContext(DbContextOptions<BugEngineDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Browser> Browser { get; set; } = null!;
        public virtual DbSet<Bug> Bug { get; set; } = null!;
        public virtual DbSet<BugType> BugType { get; set; } = null!;
        public virtual DbSet<Module> Module { get; set; } = null!;
        public virtual DbSet<OperateLog> OperateLog { get; set; } = null!;
        public virtual DbSet<Os> Os { get; set; } = null!;
        public virtual DbSet<Product> Product { get; set; } = null!;
        public virtual DbSet<PublishVersion> PublishVersion { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bug>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(m => m.Attachments).HasConversion<CompositStringToStringValueConverter, CompositStringToStringValueComparer>();
                entity.Property(m => m.Module).HasConversion<EntityValueConverter<BugEngineDbContext, Module>, EntityValueComparer<Module>>();
                entity.Property(m => m.Product).HasConversion<EntityValueConverter<BugEngineDbContext, Product>, EntityValueComparer<Product>>();
                entity.Property(m => m.Keywords).HasConversion<CompositStringToStringValueConverter, CompositStringToStringValueComparer>();
                entity.Property(m => m.Resolution).HasConversion<BugResolutionConverter, BugResolutionComparer>();
                entity.Property(m => m.Type).HasConversion<EntityValueConverter<BugEngineDbContext, BugType>, EntityValueComparer<BugType>>();
                entity.Property(m => m.Os).HasConversion<EntityValueConverter<BugEngineDbContext, Os>, EntityValueComparer<Os>>();
                entity.Property(m => m.Browser).HasConversion<EntityValueConverter<BugEngineDbContext, Browser>, EntityValueComparer<Browser>>();
                entity.Property(m => m.Cclist).HasConversion<CompositStringToInt32ValueConverter, CompositStringToInt32ValueComparer>();
                entity.Property(m => m.AffectVersions).HasConversion<EntityCollectionValueConverter<BugEngineDbContext, PublishVersion>, EntityCollectionValueComparer<PublishVersion>>();
                entity.Property(m => m.ResolvedVersion).HasConversion<EntityValueConverter<BugEngineDbContext, PublishVersion>, EntityValueComparer<PublishVersion>>();
                entity.Property(m => m.RelatedBugs).HasConversion<EntityCollectionValueConverter<BugEngineDbContext, Bug>, EntityCollectionValueComparer<Bug>>();
                entity.Property(m => m.RelatedCases).HasConversion<CompositStringToInt32ValueConverter, CompositStringToInt32ValueComparer>();
                entity.Property(m => m.OperationLogs).HasConversion<EntityCollectionValueConverter<BugEngineDbContext, OperateLog>, EntityCollectionValueComparer<OperateLog>>();
            });

            modelBuilder.Entity<OperateLog>(entity =>
            {
                entity.Property(e => e.Resolution).HasConversion<BugResolutionConverter, BugResolutionComparer>();
            });

            modelBuilder.Entity<PublishVersion>(entity =>
            {
                entity.Property(m => m.Product).HasConversion<EntityValueConverter<BugEngineDbContext, Product>, EntityValueComparer<Product>>();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}