using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessSchedularSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessInformation info = JsonConvert.DeserializeObject<ProcessInformation>(System.IO.File.ReadAllText(@"Processes.json"));
            ProcessSchedularBaseClass _schedular = SchedularFactory.CreateSchedular(info.AlgorithmName);
            Console.WriteLine("_________________________________________________________________");
            Console.WriteLine("Executing Algorithm '{0}'", _schedular);
            Console.WriteLine("_________________________________________________________________");
            _schedular.ReadyQueue = info.Processes;
            _schedular.SortProcessQueue();
            _schedular.Quanta = info.Quanta;
            _schedular.ExecuteAlgorithm();
            _schedular.DisplayFinalInformation();
            Console.ReadKey();
        }
    }

    class ProcessInformation
    {
        public string AlgorithmName { get; set; }
        public List<Process> Processes { get; set; }
        public int Quanta { get; set; }
    }
}