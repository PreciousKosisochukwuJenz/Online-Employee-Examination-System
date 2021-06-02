using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExaminationSystem.DAL.Entity
{
    public class Permission
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int? ParentID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        [ForeignKey("ParentID")]
        public virtual Permission Parent { get; set; }
    }
}
