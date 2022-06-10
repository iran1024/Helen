using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HelenServer.BugEngine.Dal
{
    public partial class Product
    {
        [Key]
        public int Id { get; set; }
        [StringLength(16)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
    }
}