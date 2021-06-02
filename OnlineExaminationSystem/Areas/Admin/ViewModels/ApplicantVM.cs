using OnlineExaminationSystem.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Areas.Admin.ViewModels
{
    public class ApplicantVM
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
        public Byte[] Photo { get; set; }
        public int? VaccanyID { get; set; }
        public string Vaccancy { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
    }
}