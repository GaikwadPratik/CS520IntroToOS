using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTerminal
{
    class Program
    {
        static void Main(string[] args)
        {
            BusTerminalConfiguration _configuration = new BusTerminalConfiguration();
            if (File.Exists("SimulationConfiguration.json"))
                _configuration = JsonConvert.DeserializeObject<BusTerminalConfiguration>(File.ReadAllText("SimulationConfiguration.json"));

            BusStopSimulation _simulation = new BusStopSimulation();
            _simulation.Configuration = _configuration;
            _simulation.Initialize();
            _simulation.SimulateEvents();
        }
    }

    public class BusTerminalConfiguration
    {
        public int BusDriveTime { get; set; } = 30;

        /// <summary>
        /// This is equal to 1/mean_arrival_rate
        /// </summary>
        public int MeanInterArrivalRate { get; set; } = 12;

        public int BoardingTime { get; set; } = 2;

        public int TotalNumberofBusStops { get; set; } = 15;

        public int TimerInterval { get; set; } = 10;

        public bool SinglePersonIncrement { get; set; } = true;

        public int MaxSimulationTime { get; set; } = 7200;
    }
}
