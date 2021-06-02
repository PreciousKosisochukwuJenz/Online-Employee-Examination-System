using OnlineExaminationSystem.DAL.Entity;
using OnlineExaminationSystem.DAL.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExaminationSystem.DAL.DataConnection
{
    public class OnlineExaminationDatabaseEntities : DbContext
    {
        public OnlineExaminationDatabaseEntities() : base("name=OnlineExaminationDatabaseEntities")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<OnlineExaminationDatabaseEntities, Configuration>());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<ApplicationSettings> ApplicationSettings { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Vaccancy> Vaccancies { get; set; }
        public virtual DbSet<Applicant> Applicants { get; set; }
        public virtual DbSet<QuestionBank> QuestionBanks { get; set; }
        public virtual DbSet<OptionBank> OptionBanks { get; set; }
        public virtual DbSet<AnswerBank> AnswerBanks { get; set; }
        public virtual DbSet<VaccancyQuestionMapping> VaccancyQuestionMappings { get; set; }
        public virtual DbSet<Assessment> Assessments { get; set; }
        public virtual DbSet<TestXPaper> TestXPapers { get; set; }
        public virtual DbSet<ApplicantAssessmentScore> ApplicantAssessmentScores { get; set; }


    }
}
