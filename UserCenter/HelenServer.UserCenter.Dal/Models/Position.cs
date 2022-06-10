using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HelenServer.UserCenter.Dal
{
    public partial class Position
    {
        [Key]
        public int Id { get; set; }
        [StringLength(32)]
        [Unicode(false)]
        public string? PositionName { get; set; }
    }
}
