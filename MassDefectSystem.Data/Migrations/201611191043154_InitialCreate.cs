namespace MassDefectSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Anomalies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OriginPlanetId = c.Int(),
                        TeleportPlanetId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Planets", t => t.OriginPlanetId)
                .ForeignKey("dbo.Planets", t => t.TeleportPlanetId)
                .Index(t => t.OriginPlanetId)
                .Index(t => t.TeleportPlanetId);
            
            CreateTable(
                "dbo.Planets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        SunId = c.Int(),
                        SolarSystemId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SolarSystems", t => t.SolarSystemId)
                .ForeignKey("dbo.Stars", t => t.SunId)
                .Index(t => t.SunId)
                .Index(t => t.SolarSystemId);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        HomePlanetId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Planets", t => t.HomePlanetId)
                .Index(t => t.HomePlanetId);
            
            CreateTable(
                "dbo.SolarSystems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Stars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        SolarSystemId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SolarSystems", t => t.SolarSystemId)
                .Index(t => t.SolarSystemId);
            
            CreateTable(
                "dbo.AnomalyVictims",
                c => new
                    {
                        AnomalyId = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AnomalyId, t.PersonId })
                .ForeignKey("dbo.Anomalies", t => t.AnomalyId, cascadeDelete: true)
                .ForeignKey("dbo.People", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.AnomalyId)
                .Index(t => t.PersonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnomalyVictims", "PersonId", "dbo.People");
            DropForeignKey("dbo.AnomalyVictims", "AnomalyId", "dbo.Anomalies");
            DropForeignKey("dbo.Anomalies", "TeleportPlanetId", "dbo.Planets");
            DropForeignKey("dbo.Stars", "SolarSystemId", "dbo.SolarSystems");
            DropForeignKey("dbo.Planets", "SunId", "dbo.Stars");
            DropForeignKey("dbo.Planets", "SolarSystemId", "dbo.SolarSystems");
            DropForeignKey("dbo.People", "HomePlanetId", "dbo.Planets");
            DropForeignKey("dbo.Anomalies", "OriginPlanetId", "dbo.Planets");
            DropIndex("dbo.AnomalyVictims", new[] { "PersonId" });
            DropIndex("dbo.AnomalyVictims", new[] { "AnomalyId" });
            DropIndex("dbo.Stars", new[] { "SolarSystemId" });
            DropIndex("dbo.People", new[] { "HomePlanetId" });
            DropIndex("dbo.Planets", new[] { "SolarSystemId" });
            DropIndex("dbo.Planets", new[] { "SunId" });
            DropIndex("dbo.Anomalies", new[] { "TeleportPlanetId" });
            DropIndex("dbo.Anomalies", new[] { "OriginPlanetId" });
            DropTable("dbo.AnomalyVictims");
            DropTable("dbo.Stars");
            DropTable("dbo.SolarSystems");
            DropTable("dbo.People");
            DropTable("dbo.Planets");
            DropTable("dbo.Anomalies");
        }
    }
}
