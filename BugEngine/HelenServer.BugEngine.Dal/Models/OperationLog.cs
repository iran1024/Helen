using HelenServer.BugEngine.Contracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelenServer.BugEngine.Dal
{
    public partial class OperateLog
    {
        [Key]
        public int Id { get; set; }
        public int Operator { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime OperatedTime { get; set; }
        public OperationType OperationType { get; set; }
        public BugResolution? Resolution { get; set; }
        [Unicode(false)]
        public string? Remark { get; set; }
    }
}