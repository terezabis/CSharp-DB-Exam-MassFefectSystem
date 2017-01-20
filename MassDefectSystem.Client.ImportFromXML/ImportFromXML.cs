namespace MassDefectSystem.Client.ImportFromXML
{
    using System;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using Data;
    using Models;
    using System.Linq;
    using System.Collections.Generic;

    class ImportFromXML
    {
        static void Main(string[] args)
        {
            var xml = XDocument.Load(NewAnomaliesPath);
            var anomalies = xml.XPathSelectElements("anomalies/anomaly");

            var context = new MassDefectSystemContext();
            foreach (var anomaly in anomalies)
            {
                ImportAnomalyAndVictims(anomaly, context);
            }
        }

        private static void ImportAnomalyAndVictims(XElement anomalyNode, MassDefectSystemContext context)
        {
            var originPlanetName = anomalyNode.Attribute("origin-planet");
            var teleportPlanetName = anomalyNode.Attribute("teleport-planet");

            if (originPlanetName == null || teleportPlanetName == null)
            {
                Console.WriteLine(ImportErrorMessage);
                return;
            }

            var anomalyEntity = new Anomaly
            {
                OriginPlanet = GetPlanetByName(originPlanetName.Value, context),
                TeleportPlanet = GetPlanetByName(teleportPlanetName.Value, context)
            };

            if (anomalyEntity.OriginPlanet == null || anomalyEntity.TeleportPlanet == null)
            {
                Console.WriteLine(ImportErrorMessage);
                return;
            }

            context.Anomalies.Add(anomalyEntity);
            Console.WriteLine(ImportUnnamedEntitySuccessMessage);

            var victims = anomalyNode.XPathSelectElements("victims/victim");
            foreach (var victim in victims)
            {
                ImportVictim(victim, context, anomalyEntity);
            }

            context.SaveChanges();
        }

        private static void ImportVictim(XElement victimNode, MassDefectSystemContext context, Anomaly anomaly)
        {
            var name = victimNode.Attribute("name");

            var personalEntity = GetPersonByName(name.Value, context);

            anomaly.Persons.Add(personalEntity);
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

        private const string NewAnomaliesPath = @"../../../datasets/new-anomalies.xml";

        private const string ImportUnnamedEntitySuccessMessage = "Successfully imported anomaly.";

        private const string ImportErrorMessage = "Error: Invalid data.";
    }
}
