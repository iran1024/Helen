using HelenServer.Data.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace HelenServer.UserCenter.Dal
{
    public partial class UserCenterDbContext : DbContext
    {
        public UserCenterDbContext(DbContextOptions<UserCenterDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Department> Department { get; set; } = null!;
        public virtual DbSet<Position> Position { get; set; } = null!;
        public virtual DbSet<User> User { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_DEPARTMENT")
                    .IsClustered(false);

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_POSITION")
                    .IsClustered(false);

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_USER")
                    .IsClustered(false);

                entity.Property(e => e.Id).HasValueGenerator<UniqueIdGenerator>().ValueGeneratedOnAdd();

                entity.Property(e => e.Sex).IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optsBuilder)
        {
            optsBuilder.UseSqlServer("Data Source=172.25.13.69;Initial Catalog=Helen;User ID=sa;Password=iyiran1024.;Trust Server Certificate=True")
                .AddInterceptors(new DefaultIntercepter());
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        #region CustomMethod

        private IDictionary<int, string> _deptMapper = null!;
        private IDictionary<int, string> _positionMapper = null!;

        public void InitPersistedModel()
        {
            _deptMapper = Department.ToDictionary(k => k.Id, v => v.DepartmentName!);
            _positionMapper = Position.ToDictionary(k => k.Id, v => v.PositionName!);
        }

        public (int Id, string Name)? GetDepartment(int? id, string? orName)
        {
            if (id is null && orName is not null)
            {
                var kv = _deptMapper.FirstOrDefault(n => n.Value == orName);

                return (kv.Key, kv.Value);
            }
            else if (id is not null && orName is null)
            {
                if (_deptMapper.TryGetValue(id.Value, out var v))
                {
                    return (id.Value, v!);
                }

                return null;
            }

            return null;
        }

        public (int Id, string Name)? GetPosition(int? id, string? orName)
        {
            if (id is null && orName is not null)
            {
                var kv = _positionMapper.FirstOrDefault(n => n.Value == orName);

                return (kv.Key, kv.Value);
            }
            else if (id is not null && orName is null)
            {
                if (_positionMapper.TryGetValue(id.Value, out var v))
                {
                    return (id.Value, v!);
                }

                return null;
            }

            return null;
        }
        #endregion
    }
}