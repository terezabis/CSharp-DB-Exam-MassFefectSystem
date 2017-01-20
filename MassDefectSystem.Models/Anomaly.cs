using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassDefectSystem.Models
{
    public class Anomaly
    {
        public Anomaly()
        {
            this.Persons = new HashSet<Person>();
        }
        
        [Key]
        public int Id { get; set; }

        public int? OriginPlanetId { get; set; }

        public int? TeleportPlanetId { get; set; }

        [ForeignKey("OriginPlanetId")]
        public virtual Planet OriginPlanet { get; set; }

        [ForeignKey("TeleportPlanetId")]
        public virtual Planet TeleportPlanet { get; set; }

        public virtual ICollection<Person> Persons { get; set; }
    }
}
