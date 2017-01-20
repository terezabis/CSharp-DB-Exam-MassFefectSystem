

namespace MassDefectSystem.Client.ExportToJSON
{
    using System;
    using Data;
    using System.Linq;
    using Newtonsoft.Json;
    using System.IO;

    class ExportToJSON
    {
        static void Main(string[] args)
        {
            var context = new MassDefectSystemContext();

            ExportPlanetsWichAreNotAnomalyOrigins(context);

            ExportPeopleWichHaveNotBeenVictims(context);

            ExportAnomaly(context);
        }

        private static void ExportPeopleWichHaveNotBeenVictims(MassDefectSystemContext context)
        {
            var exportedPeople = context.Persons
                .Where(person => !person.Anomalies.Any())
                .Select(person => new
                {
                    name = person.Name,
                    homePlanet = new
                    {
                        name = person.HomePlanet.Name
                    }
                });

            var peopleAsJson = JsonConvert.SerializeObject(exportedPeople, Formatting.Indented);
            File.WriteAllText("../../../results/people.json", peopleAsJson);
        }

        private static void ExportPlanetsWichAreNotAnomalyOrigins(MassDefectSystemContext context)
        {
            var exportedPlanets = context.Planets
                .Where(planet => !planet.OriginAnomalies.Any())
                .Select(planet => new
                {
                    name = planet.Name
                });

            var planetAsJson = JsonConvert.SerializeObject(exportedPlanets, Formatting.Indented);
            File.WriteAllText("../../../results/planets.json", planetAsJson);
        }

        private static void ExportAnomaly(MassDefectSystemContext context)
        {
            var exportedAnomaly = context.Anomalies
                .Where(anomaly => anomaly.Persons.Any())
                .OrderByDescending(anomaly => anomaly.Persons.Count)
                .Select(anomaly => new
                {
                    id = anomaly.Id,
                    originPlanet = new
                    {
                        name = anomaly.OriginPlanet.Name
                    },
                    teleportPlanet = new
                    {
                        name = anomaly.TeleportPlanet.Name
                    },
                    victimsCount = anomaly.Persons.Count
                })
                .Take(1);

            var anomalyToJson = JsonConvert.SerializeObject(exportedAnomaly, Formatting.Indented);
            File.WriteAllText("../../../results/anomaly.json", anomalyToJson);
        }

        
    }
}
