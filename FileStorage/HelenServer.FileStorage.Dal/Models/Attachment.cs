using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelenServer.FileStorage.Dal
{
    [Table("Attachment")]
    public class Attachment
    {
        [Key]
        [StringLength(64)]
        [Unicode(false)]
        public string Id { get; set; } = null!;

        [StringLength(256)]
        [Unicode(false)]
        public string Url { get; set; } = null!;

        [StringLength(8)]
        [Unicode(false)]
        public string GroupName { get; set; } = string.Empty;

        [StringLength(64)]
        [Unicode(false)]
        public string Name { get; set; } = null!;

        [StringLength(16)]
        [Unicode(false)]
        public string Hash { get; set; } = null!;
    }
}