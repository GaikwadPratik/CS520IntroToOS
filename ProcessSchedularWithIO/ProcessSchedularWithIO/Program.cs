using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessSchedularWithIO
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
            _schedular.CPUBurstTimes = info.BurstTimes;
            _schedular.SortProcessQueue();
            _schedular.Initialize();
            _schedular.Quanta = info.Quanta;
            _schedular.ExecuteAlgorithm();
            _schedular.DisplayFinalInformation();

            //int n = 1;
            //while (n < info.Quanta)
            //{
            //    _schedular = SchedularFactory.CreateSchedular(info.AlgorithmName);
            //    Console.WriteLine("_________________________________________________________________");
            //    Console.WriteLine("Executing Algorithm '{0}'", _schedular);
            //    Console.WriteLine("_________________________________________________________________");
            //    _schedular.ReadyQueue = info.Processes;
            //    _schedular.CPUBurstTimes = info.BurstTimes;
            //    _schedular.SortProcessQueue();
            //    _schedular.Initialize();
            //    _schedular.Quanta = info.Quanta;
            //    _schedular.ExecuteAlgorithm();
            //    _schedular.DisplayFinalInformation();
            //}
            Console.ReadKey();
        }
    }

    class ProcessInformation
    {
        public string AlgorithmName { get; set; }
        public List<Process> Processes { get; set; }
        public List<int> BurstTimes { get; set; }
        public int Quanta { get; set; }
        public double Alpha { get; set; }
        public int IOTime { get; set; }
    }
}
