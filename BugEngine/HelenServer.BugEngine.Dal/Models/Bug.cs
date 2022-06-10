using HelenServer.BugEngine.Contracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelenServer.BugEngine.Dal
{
    public partial class Bug
    {
        [Key]
        public int Id { get; set; }
        [StringLength(128)]
        [Unicode(false)]
        public string Title { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public virtual Module? Module { get; set; }
        public int? Demand { get; set; }

        [StringLength(64)]
        [Unicode(false)]
        public string[] Keywords = Array.Empty<string>();
        public int Severity { get; set; }
        public int Priority { get; set; }
        public virtual BugType Type { get; set; } = null!;
        [Column("OS")]
        public virtual Os? Os { get; set; }
        public virtual Browser? Browser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ExpirationTime { get; set; }
        [Unicode(false)]
        public string? Reproduce { get; set; }
        public BugStatus Status { get; set; }
        public int ActivedCount { get; set; } = 0;
        [Column(TypeName = "datetime")]
        public DateTime? LastActivedTime { get; set; }
        public bool IsConfirmed { get; set; } = false;
        [Column("CCList")]
        [StringLength(64)]
        [Unicode(false)]
        public virtual ICollection<int> Cclist { get; set; } = new List<int>();
        public int Creator { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedTime { get; set; }
        [StringLength(64)]
        [Unicode(false)]
        public virtual ICollection<PublishVersion> AffectVersions { get; set; } = new List<PublishVersion>();
        public int Assignment { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AssignmentTime { get; set; }
        public int? ResolvedBy { get; set; }

        public virtual BugResolution? Resolution { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ResolvedTime { get; set; }
        public virtual PublishVersion? ResolvedVersion { get; set; }
        public int? ClosedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ClosedTime { get; set; }
        public int? DuplicateId { get; set; }
        [StringLength(128)]
        [Unicode(false)]
        public virtual ICollection<Bug> RelatedBugs { get; set; } = new List<Bug>();
        [StringLength(64)]
        [Unicode(false)]
        public virtual ICollection<int> RelatedCases { get; set; } = new List<int>();
        public int? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
        [StringLength(512)]
        [Unicode(false)]
        public virtual string[] Attachments { get; set; } = Array.Empty<string>();
        [StringLength(64)]
        [Unicode(false)]
        public virtual ICollection<OperateLog> OperationLogs { get; set; } = new List<OperateLog>();
    }
}