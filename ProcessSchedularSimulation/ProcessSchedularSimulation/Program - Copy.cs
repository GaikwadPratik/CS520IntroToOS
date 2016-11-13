using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApplication1
{
    enum Algorithms
    {
        FCFS,
        SJF,
        PRIORITY,
        RR
    }
    class Program
    {
        static void Main(string[] args)
        {
            string _strFileName = @"TextFile1.txt";
            //----------------------------------------Reading I/O File--------------------------------------
            string _strDirectoryName = Environment.CurrentDirectory.ToString();   // returns the directory of the exe file
            if (File.Exists(Path.Combine(_strDirectoryName, _strFileName))) //checking if the input files exists
                Console.WriteLine("File Exists");
            else
            {
                Console.WriteLine("File Not Found");
                Console.WriteLine("_________________________________________________________________");
                return;
            }
            Console.WriteLine("_________________________________________________________________");
            //----------------------------------------Data Into List--------------------------------------
            string FileText = File.ReadAllText(Path.Combine(_strDirectoryName, _strFileName)); //reading all the text in the input file
            string[] lines = FileText.Split('\n'); //splitting the lines
            List<Process> processes = new List<Process>();
            foreach (string line in lines)
            {
                string[] tabs = line.Split('\t');//splitting the tabs to get objects' variables
                Process x = new Process(tabs[0], int.Parse(tabs[1]), int.Parse(tabs[2]), int.Parse(tabs[3]));//creating object
                processes.Add(x);//adding object to the list
            }

            //   ----------------------------------------Sorting The List--------------------------------------
            processes = SortProcessByAlgorithm(Algorithms.PRIORITY, processes);

            Console.WriteLine("Processes After Sorting");
            Console.WriteLine("_________________________________________________________________");
            Console.WriteLine("Name\tArrival\tBrust\tPriority");
            for (int i = 0; i < processes.Count; i++)
            {
                Console.Write(processes[i].Name + "\t" + processes[i].ArrivalTime + "\t" + processes[i].Brust + "\t" + processes[i].Priority);
                Console.WriteLine();
            }
            Console.WriteLine("_________________________________________________________________");
            //----------------------------------------Gantt Chart--------------------------------------
            Console.WriteLine("Gantt Chart");
            Console.WriteLine("_________________________________________________________________");
            int counter = 0;
            for (int i = 0; i < processes.Count; i++)
            {
                Console.Write(processes[i].Name + "\t");
                if (processes[i].ArrivalTime < counter)
                    printSpaces(counter);
                else
                {
                    printSpaces(processes[i].ArrivalTime);
                    counter = processes[i].ArrivalTime;
                }
                printHashes(processes[i].Brust);
                counter += processes[i].Brust;
                Console.WriteLine();
            }
            Console.WriteLine("_________________________________________________________________");
            //-----------------------------------Completing Data And final Table-------------------------
            int clock = 0, totalwait = 0, totalturnAround = 0;
            for (int i = 0; i < processes.Count; i++)
            {
                if (processes[i].ArrivalTime > clock)
                {
                    processes[i].Start = processes[i].ArrivalTime;
                    clock += processes[i].Start - processes[i].ArrivalTime;
                    clock += processes[i].Brust;

                }
                else
                {
                    if (i > 0)
                        processes[i].Start = processes[i - 1].End;
                    clock += processes[i].Brust;
                }
                if (processes[i].Start > processes[i].ArrivalTime)
                    processes[i].Wait = processes[i].Start - processes[i].ArrivalTime;
                else processes[i].Wait = 0;
                processes[i].End = processes[i].Start + processes[i].Brust;
                processes[i].TurnAround = processes[i].Wait + processes[i].Brust;
                totalwait += processes[i].Wait;
                totalturnAround += processes[i].TurnAround;
            }
            Console.WriteLine("Name\tArrival\tBrust\tStart\tEnd\tWait\tturnaround");
            for (int i = 0; i < processes.Count; i++)
            {
                Console.Write(processes[i].Name + "\t" + processes[i].ArrivalTime + "\t" + processes[i].Brust + "\t" + processes[i].Start + "\t" + processes[i].End + "\t" + processes[i].Wait + "\t" + processes[i].TurnAround);
                Console.WriteLine();
            }
            double att = 0, awt = 0;
            awt = (double)totalwait / (double)processes.Count;
            att = (double)totalturnAround / (double)processes.Count;
            Console.WriteLine("A.W.T= " + awt + "\t A.T.T= " + att);
            Console.ReadKey();
        }
        public static void printSpaces(int counter)
        {
            for (int i = 0; i < counter; i++)
            {
                Console.Write(" ");
            }
        }
        public static void printHashes(int brust)
        {
            for (int i = 0; i < brust; i++)
            {
                Console.Write("#");
            }
        }

        private static List<Process> SortProcessByAlgorithm(Algorithms algorithm, List<Process> ListOfProcesses)
        {
            try
            {
                switch (algorithm)
                {
                    case Algorithms.FCFS:
                    case Algorithms.RR:
                    default:
                        ListOfProcesses = ListOfProcesses.OrderBy(x => x.ArrivalTime).ToList();
                        break;
                    case Algorithms.SJF:
                        ListOfProcesses = ListOfProcesses.OrderBy(x => x.Brust).ToList();
                        break;
                    case Algorithms.PRIORITY:
                        ListOfProcesses.Sort((x, y) =>
                        {
                            if ((x.Priority < y.Priority) || (x.Priority.Equals(y.Priority) && x.Brust < y.Brust))
                                return -1;
                            else
                                return 0;
                        });
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return ListOfProcesses;
        }

    }

    class Process
    {
        public string Name { get; set; } = "P0";
        public int ArrivalTime { get; set; } = 0;
        public int Brust { get; set; } = 0;
        public int Priority { get; set; } = 5;
        public int Wait { get; set; } = 0;
        public int End { get; set; } = int.MaxValue;
        public int Start { get; set; } = 0;
        public int TurnAround { get; set; } = 0;


        public Process(string name, int arrivalTime, int brust, int priority)
        {
            Name = name;
            ArrivalTime = arrivalTime;
            Brust = brust;
            Priority = priority;
        }
    }
}
