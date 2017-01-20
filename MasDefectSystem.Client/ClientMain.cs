using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassDefectSystem.Data;
using System.Data.Entity;

namespace MasDefectSystem.Client
{
    class ClientMain
    {
        static void Main(string[] args)
        {
            var context = new MassDefectSystemContext();
            context.Database.Initialize(true);
            
        }
    }
}
