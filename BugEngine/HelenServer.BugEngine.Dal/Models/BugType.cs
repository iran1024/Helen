using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HelenServer.BugEngine.Dal
{
    public partial class BugType
    {
        [Key]
        public int Id { get; set; }
        [StringLength(16)]
        [Unicode(false)]
        public string TypeName { get; set; } = null!;
    }
}