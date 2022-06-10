using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelenServer.UserCenter.Dal
{
    public class User
    {
        [Key]
        [StringLength(32)]
        [Unicode(false)]
        public string Id { get; set; } = null!;
        [StringLength(32)]
        [Unicode(false)]
        public string Username { get; set; } = null!;
        [StringLength(32)]
        [Unicode(false)]
        public string Password { get; set; } = null!;
        [StringLength(128)]
        [Unicode(false)]
        public string? Avatar { get; set; }
        [StringLength(16)]
        [Unicode(false)]
        public string? Name { get; set; }
        public int Sex { get; set; }
        public int? Department { get; set; }
        public int? Position { get; set; }
        [StringLength(16)]
        [Unicode(false)]
        public string? Roles { get; set; }
        public int? JobNumer { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? InductionDate { get; set; }
        public int Status { get; set; }
        [StringLength(32)]
        [Unicode(false)]
        public string? Email { get; set; }
        [Column("LastIP")]
        [StringLength(16)]
        [Unicode(false)]
        public string? LastIp { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastLoginTime { get; set; }
    }
}