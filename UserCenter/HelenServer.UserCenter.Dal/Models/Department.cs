using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HelenServer.UserCenter.Dal
{
    public partial class Department
    {
        [Key]
        public int Id { get; set; }
        [StringLength(32)]
        [Unicode(false)]
        public string? DepartmentName { get; set; }
    }
}