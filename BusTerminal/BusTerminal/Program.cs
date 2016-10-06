using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTerminal
{
    class Program
    {
        static void Main(string[] args)
        {
            BusStopSimulation _simulation = new BusStopSimulation();
            _simulation.Initialize();
            _simulation.SimulateEvents();
        }
    }
}
