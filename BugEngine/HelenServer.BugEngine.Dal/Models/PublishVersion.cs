using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HelenServer.BugEngine.Dal
{
    public partial class PublishVersion : IEqualityComparer<PublishVersion>
    {
        [Key]
        public int Id { get; set; }
        [StringLength(16)]
        [Unicode(false)]
        public string Version { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;

        public bool Equals(PublishVersion? x, PublishVersion? y)
        {
            return x is not null && y is not null &&
                   x.Id == y.Id &&
                   x.Version == y.Version &&
                   EqualityComparer<Product>.Default.Equals(x.Product, y.Product);
        }

        public int GetHashCode([DisallowNull] PublishVersion obj)
        {
            return HashCode.Combine(Id, Version, Product);
        }
    }
}