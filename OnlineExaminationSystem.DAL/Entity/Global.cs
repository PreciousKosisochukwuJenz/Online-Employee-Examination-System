using OnlineExaminationSystem.DAL.DataConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExaminationSystem.DAL.Entity
{

    public class Global
    {
        OnlineExaminationDatabaseEntities db = new OnlineExaminationDatabaseEntities();

        public static int AuthenticatedUserID { get; set; }
        public static int AuthenticatedApplicantID { get; set; }
        public static string ErrorMessage { get; set; }
        public Applicant ApplicantInfo()
        {
             return db.Applicants.FirstOrDefault(x => x.Id == AuthenticatedApplicantID);
        }
    }

    public enum Gender
    {
        Male = 1,
        Female
    }
    public enum State
    {
        Abia = 1,
        Adamawa,
        Akwa_Ibom,
        Anambra,
        Bauchi,
        Bayelsa,
        Benue,
        Borno,
        Cross_River,
        Delta,
        Ebonyi,
        Edo,
        Ekiti,
        Enugu,
        Gombe,
        Imo,
        Jigawa,
        Kaduna,
        Kano,
        Kastina,
        Kebbi,
        Kogi,
        Kwara,
        Lagos,
        Nasarawa,
        Niger,
        Ogun,
        Ondo,
        Osun,
        Oyo,
        Plateau,
        Rivers,
        Sokoto,
        Taraba,
        Yobe,
        Zamfara,
        Abuja
    }
    public enum Nationality
    {
        Nigeria = 1
    }
    public enum QuestionType
    {
        [EnumDisplayName(DisplayName = "MULTI CHOICE")]
        Multi_Choice = 1,
        [EnumDisplayName(DisplayName = "FILL IN THE BLANK")]
        Fill_in_the_blank
    }
    public class EnumDisplayNameAttribute : Attribute
    {
        private string _displayName;
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                _displayName = value;
            }
        }
    }

    public static class EnumExtensions
    {

        public static string DisplayName(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            EnumDisplayNameAttribute attribute
                = Attribute.GetCustomAttribute(field, typeof(EnumDisplayNameAttribute))
            as EnumDisplayNameAttribute;

            return attribute == null ? value.ToString() : attribute.DisplayName;
        }
    }
}
