namespace OnlineExaminationSystem.DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OnlineExaminationSystem.DAL.DataConnection.OnlineExaminationDatabaseEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

    }
}
