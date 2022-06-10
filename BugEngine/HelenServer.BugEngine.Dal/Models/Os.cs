using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelenServer.BugEngine.Dal
{
    [Table("OS")]
    public partial class Os
    {
        [Key]
        public int Id { get; set; }
        [StringLength(32)]
        [Unicode(false)]
        public string Version { get; set; } = null!;
    }
}