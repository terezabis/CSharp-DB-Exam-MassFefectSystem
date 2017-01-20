namespace MassDefectSystem.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Models;

    public class MassDefectSystemContext : DbContext
    {

        public MassDefectSystemContext()
            : base("name=MassDefectSystemContext")
        {
        }

        public IDbSet<SolarSystem> SolarSystems { get; set; }

        public IDbSet<Star> Stars { get; set; }

        public IDbSet<Planet> Planets { get; set; }

        public IDbSet<Person> Persons { get; set; }

        public IDbSet<Anomaly> Anomalies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Anomaly>()
               .HasMany<Person>(a => a.Persons)
               .WithMany(p => p.Anomalies)
               .Map(ap =>
               {
                   ap.MapLeftKey("AnomalyId");
                   ap.MapRightKey("PersonId");
                   ap.ToTable("AnomalyVictims");
               });

            modelBuilder.Entity<Anomaly>()
                .Property(a => a.OriginPlanetId)
                .HasColumnOrder(2);

            base.OnModelCreating(modelBuilder);
        }
    }

}