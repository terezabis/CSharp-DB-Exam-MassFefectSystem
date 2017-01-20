using MassDefectSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MassDefectSystem.Client.ExportToXML
{
    class ExportToXML
    {
        static void Main(string[] args)
        {
            var context = new MassDefectSystemContext();
            var exportedAnomalies = context.Anomalies
                .OrderBy(anomaly => anomaly.Id)
                .Select(anomaly => new
                {
                    id = anomaly.Id,
                    originPlanetName = anomaly.OriginPlanet.Name,
                    teleportPlanetName = anomaly.TeleportPlanet.Name,
                    victims = anomaly.Persons
                });

            var xmlDocument = new XElement("anomalies");

            foreach (var exportedAnomaly in exportedAnomalies)
            {
                var anomalyNode = new XElement("anomaly");
                anomalyNode.Add(new XAttribute("id", exportedAnomaly.id));
                anomalyNode.Add(new XAttribute("origin-planet", exportedAnomaly.originPlanetName));
                anomalyNode.Add(new XAttribute("teleport-planet", exportedAnomaly.teleportPlanetName));

                var victimsNode = new XElement("victims");
                foreach (var victim in exportedAnomaly.victims)
                {
                    var victimNode = new XElement("victim");
                    victimNode.Add(new XAttribute("name", victim.Name));
                    victimsNode.Add(victimNode);
                }
                anomalyNode.Add(victimsNode);
                xmlDocument.Add(anomalyNode);
            }
            
            xmlDocument.Save("../../../results/anomalies.xml");
        }
    }
}
