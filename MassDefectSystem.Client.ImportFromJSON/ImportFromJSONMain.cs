using MassDefectSystem.Data;
using MassDefectSystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassDefectSystem.Client.ImportFromJSON
{
    public class ImportFromJSONMain
    {
        static void Main(string[] args)
        {
            //var context = new MassDefectSystemContext();
            //context.Database.Initialize(true);

            ImportSolarSystems();
            ImportStars();
            ImportPlanets();
            ImportPersons();
            ImportAnomalies();
            ImportAnomalyVictims();
        }

        private const string SolarSystemPath = @"C:\Users\Globul\Documents\SoftUni\C# DB Advanced\Exersices\MassDefectSystem\datasets\solar-systems.json";

        private const string StarsPath = @"C:\Users\Globul\Documents\SoftUni\C# DB Advanced\Exersices\MassDefectSystem\datasets\stars.json";

        private const string PlanetsPath = @"C:\Users\Globul\Documents\SoftUni\C# DB Advanced\Exersices\MassDefectSystem\datasets\planets.json";

        private const string PersonsPath = @"C:\Users\Globul\Documents\SoftUni\C# DB Advanced\Exersices\MassDefectSystem\datasets\persons.json";

        private const string AnomaliesPath = @"C:\Users\Globul\Documents\SoftUni\C# DB Advanced\Exersices\MassDefectSystem\datasets\anomalies.json";

        private const string AnomalyVictims = @"C:\Users\Globul\Documents\SoftUni\C# DB Advanced\Exersices\MassDefectSystem\datasets\anomaly-victims.json";

        //private const string SuccessAdded = "Successfully imported {entity} {entityName}.";

        //private const string SuccessAddedAnomaly = "Successfully imported anomaly.";

        private const string Error = "Error: Invalid data.";


        private static void ImportAnomalyVictims()
        {
            var context = new MassDefectSystemContext();
            var json = File.ReadAllText(AnomalyVictims);
            var anomalyVictims = JsonConvert.DeserializeObject<IEnumerable<AnomalyVictimsDTO>>(json);

            foreach (var anomalyVictim in anomalyVictims)
            {
                if (anomalyVictim.Id == null || anomalyVictim.Person == null)
                {
                    Console.WriteLine(Error);
                    continue;
                }

                var anomalyEntity = GetAnomalyById(Convert.ToInt32(anomalyVictim.Id), context);
                var personEntity = GetPersonByName(anomalyVictim.Person, context);

                if (anomalyEntity == null || personEntity == null)
                {
                    Console.WriteLine(Error);
                    continue;
                }
                else
                {
                    anomalyEntity.Persons.Add(personEntity);
                    
                }
            }
            context.SaveChanges();
        }


        private static void ImportAnomalies()
        {
            var context = new MassDefectSystemContext();
            var json = File.ReadAllText(AnomaliesPath);
            var anomalies = JsonConvert.DeserializeObject<IEnumerable<AnomalyDTO>>(json);

            foreach (var anomaly in anomalies)
            {
                if (anomaly.OriginPlanet == null || anomaly.TeleportPlanet == null)
                {
                    Console.WriteLine(Error);
                    continue;
                }

                var anomalyEntity = new Anomaly
                {
                    OriginPlanet = GetPlanetByName(anomaly.OriginPlanet, context),
                    TeleportPlanet = GetPlanetByName(anomaly.TeleportPlanet, context)
                };

                if (anomalyEntity.OriginPlanet == null)
                {
                    Console.WriteLine(Error);
                    continue;
                }
                if (anomalyEntity.TeleportPlanet == null)
                {
                    Console.WriteLine(Error);
                    continue;
                }
                context.Anomalies.Add(anomalyEntity);
                Console.WriteLine("Successfully imported anomaly.");

            }
            context.SaveChanges();
        }

        private static void ImportPersons()
        {
            var context = new MassDefectSystemContext();
            var json = File.ReadAllText(PersonsPath);
            var persons = JsonConvert.DeserializeObject<IEnumerable<PersonDTO>>(json);

            foreach (var person in persons)
            {
                if (person.Name == null || person.HomePlanet == null)
                {
                    Console.WriteLine(Error);
                    continue;
                }

                var personEntity = new Person
                {
                    Name = person.Name,
                    HomePlanet = GetPlanetByName(person.HomePlanet, context)
                };

                if (personEntity.HomePlanet == null)
                {
                    Console.WriteLine(Error);
                    continue;
                }

                context.Persons.Add(personEntity);
                Console.WriteLine($"Successfully imported Person {personEntity.Name}.");

            }
            context.SaveChanges();
        }

        private static void ImportPlanets()
        {
            var context = new MassDefectSystemContext();
            var json = File.ReadAllText(PlanetsPath);
            var planets = JsonConvert.DeserializeObject<IEnumerable<PlanetDTO>>(json);

            foreach (var planet in planets)
            {
                if (planet.Name == null || planet.Sun == null || planet.SolarSystem == null)
                {
                    Console.WriteLine(Error);
                    continue;
                }
                var planetEntity = new Planet
                {
                    Name = planet.Name,
                    Sun = GetSunByName(planet.Sun, context),
                    SolarSystem = GetSolarSystemByName(planet.SolarSystem, context)
                };
                if (planetEntity.Sun == null)
                {
                    Console.WriteLine(Error);
                    continue;
                }
                if (planetEntity.SolarSystem == null)
                {
                    Console.WriteLine(Error);
                    continue;
                }

                context.Planets.Add(planetEntity);
                Console.WriteLine($"Successfully imported Planet {planetEntity.Name}.");

            }
            context.SaveChanges();
        }

        private static void ImportStars()
        {
            var context = new MassDefectSystemContext();
            var json = File.ReadAllText(StarsPath);
            var stars = JsonConvert.DeserializeObject<IEnumerable<StarDTO>>(json);

            foreach (var star in stars)
            {
                if (star.Name == null || star.SolarSystem == null)
                {
                    Console.WriteLine(Error);
                    continue;
                }

                var starEntity = new Star
                {
                    Name = star.Name,
                    SolarSystem = GetSolarSystemByName(star.SolarSystem, context)
                };

                if (starEntity.SolarSystem == null)
                {
                    Console.WriteLine(Error);
                    continue;
                }

                context.Stars.Add(starEntity);
                Console.WriteLine($"Successfully imported Star {starEntity.Name}.");

            }
            context.SaveChanges();
        }

        private static void ImportSolarSystems()
        {
            var context = new MassDefectSystemContext();
            var json = File.ReadAllText(SolarSystemPath);
            var solarSystems = JsonConvert.DeserializeObject<IEnumerable<SolarSystemDTO>>(json);

            foreach (var solarSystem in solarSystems)
            {
                if (solarSystem.Name == null)
                {
                    Console.WriteLine(Error);
                    continue;
                }
                else
                {
                    var solarSystemEntity = new SolarSystem
                    {
                        Name = solarSystem.Name
                    };
                    context.SolarSystems.Add(solarSystemEntity);
                    Console.WriteLine($"Successfully imported Solar System {solarSystem.Name}");
                }
            }
            context.SaveChanges();
        }

        private static Person GetPersonByName(string personName, MassDefectSystemContext context)
        {
            var isPersonExist = context.Persons.Any(p => p.Name == personName);

            if (isPersonExist)
            {
                var personEntity = context.Persons.Where(p => p.Name == personName).First();
                return personEntity;
            }
            else
            {
                return null;
            }
        }

        private static Anomaly GetAnomalyById(int anomalyId, MassDefectSystemContext context)
        {
            var isAnomalyExist = context.Anomalies.Any(a => a.Id == anomalyId);

            if (isAnomalyExist)
            {
                var anomalyEntity = context.Anomalies.Where(a => a.Id == anomalyId).First();
                return anomalyEntity;
            }
            else
            {
                return null;
            }
        }

        private static Planet GetPlanetByName(string homePlanet, MassDefectSystemContext context)
        {
            var isPlanetExist = context.Planets.Any(s => s.Name == homePlanet);

            if (isPlanetExist)
            {
                var planetEntity = context.Planets.Where(s => s.Name == homePlanet).First();
                return planetEntity;
            }
            else
            {
                return null;
            }
        }

        private static Star GetSunByName(string sun, MassDefectSystemContext context)
        {
            var isSunExist = context.Stars.Any(s => s.Name == sun);

            if (isSunExist)
            {
                var sunEntity = context.Stars.Where(s => s.Name == sun).First();
                return sunEntity;
            }
            else
            {
                return null;
            }

        }

        private static SolarSystem GetSolarSystemByName(string solarSystem, MassDefectSystemContext context)
        {
            var isSolarSystemExist = context.SolarSystems.Any(s => s.Name == solarSystem);

            if (isSolarSystemExist)
            {
                var solarSystemEntity = context.SolarSystems.Where(s => s.Name == solarSystem).First();
                return solarSystemEntity;
            }
            else
            {
                return null;
            }
        }
    }
}
