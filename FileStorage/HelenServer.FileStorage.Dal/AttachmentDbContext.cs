using Microsoft.EntityFrameworkCore;

namespace HelenServer.FileStorage.Dal
{
    public partial class AttachmentDbContext : DbContext
    {
        public AttachmentDbContext()
        {
        }

        public AttachmentDbContext(DbContextOptions<AttachmentDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attachment> Attachments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}