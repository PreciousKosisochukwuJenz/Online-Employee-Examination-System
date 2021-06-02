using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExaminationSystem.DAL.Entity
{
    public class Applicant
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public State State { get; set; }
        public Nationality Nationality { get; set; }
        public string LGA { get; set; }
        public string Address { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public Byte[] CV { get; set; }
        public int? VaccancyID { get; set; }
        public bool IsDeleted { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public Byte[] Photo { get; set; }

        public DateTime DateCreated { get; set; }

        [ForeignKey("VaccancyID")]
        public Vaccancy Vaccancy { get; set; }

        
    }
}
