using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExaminationSystem.DAL.Entity
{
    public class RolePermission
    {
        public int Id { get; set; }
        public int? RoleID { get; set; }
        public int? PermissionID { get; set; }
        public int? PermissionParentID { get; set; }
        public bool IsAssigned { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        [ForeignKey("RoleID")]
        public virtual Role Role { get; set; }
        [ForeignKey("PermissionID")]
        public virtual Permission Permission { get; set; }
    }
}
