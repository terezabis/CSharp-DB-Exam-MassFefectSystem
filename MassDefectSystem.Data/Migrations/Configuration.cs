namespace MassDefectSystem.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MassDefectSystem.Data.MassDefectSystemContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "MassDefectSystem.Data.MassDefectSystemContext";
        }

        protected override void Seed(MassDefectSystem.Data.MassDefectSystemContext context)
        {
            
        }
    }
}
