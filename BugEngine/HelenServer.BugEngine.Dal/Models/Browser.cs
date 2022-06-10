using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HelenServer.BugEngine.Dal
{
    public partial class Browser
    {
        [Key]
        public int Id { get; set; }
        [StringLength(32)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
    }
}