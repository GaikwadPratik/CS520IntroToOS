using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessSchedularWithIO
{
    /// <summary>
    /// Base class to all the algorithms in the process scheduling. All the algorithms must inherit this class.
    /// The objects must of this class but instantiation should be done for child classes.
    /// </summary>
    class ProcessSchedularBaseClass
    {
        #region Properties
        /// <summary>
        /// The list of the processes in the queue. This list should contain only those processes which are not marked as Completed or just arrived
        /// </summary>
        public List<Process> ReadyQueue { get; set; }

        /// <summary>
        /// The list of the processes in the queue. This list should contain only those processes which are marked as Completed
        /// </summary>
        public List<Process> CompletedQueue { get; set; }

        /// <summary>
        /// The counter is used to create spaces and hashes to indicate the progress in grantt chart.
        /// This must be passed down from process to another so as to keep contunuation
        /// </summary>
        public int Counter { get; set; } = 0;

        /// <summary>
        /// Just to pass the end time of the previous process
        /// </summary>
        public int PreviousProcessEndTime { get; set; }

        /// <summary>
        /// Total wait time of the queue.
        /// </summary>
        public int TotalWaitTime { get; set; }

        /// <summary>
        /// Total turn around time of the queue.
        /// </summary>
        public int TotalTurnAroundTime { get; set; }

        /// <summary>
        /// Time quanta required for Round robin. For all other this can be ignored.
        /// </summary>
        public int Quanta { get; set; } = 60;

        /// <summary>
        /// Used for Random number generation
        /// </summary>
        public int Seed { get; set; } = 1000;

        /// <summary>
        /// Used for Random number generation
        /// </summary>
        public int Modulus { get; set; } = 65536;

        /// <summary>
        /// Used for Random number generation
        /// </summary>
        public int Increment { get; set; } = 13849;

        /// <summary>
        /// Used for Random number generation
        /// </summary>
        public int Multiplier { get; set; } = 25173;

        /// <summary>
        /// Contains the list of CPU Burst times for each of the process
        /// </summary>
        public List<int> CPUBurstTimes { get; set; }

        /// <summary>
        /// Used for Throughput
        /// </summary>
        public int ProcessTotal { get; set; } = 0;

        /// <summary>
        /// Total running time of the CPU during execution of simulation
        /// </summary>
        public int CPURunningTime { get; set; } = 0;

        /// <summary>
        /// Total running time of the CPU during execution of simulation
        /// </summary>
        public int CPUWaitingTime { get; set; } = 0;

        /// <summary>
        /// used for random number generation
        /// </summary>
        public int Alpha { get; set; } = 0;

        /// <summary>
        /// Time at which context switch happens for a process
        /// </summary>
        public int ContextSwitch { get; set; }

        /// <summary>
        /// Time required for IO processing
        /// </summary>
        public int IOTime { get; set; } = 60;

        /// <summary>
        /// Just for internal purpose
        /// </summary>
        private bool IsRR
        {
            get
            {
                return GetType().Name.Equals("RR");
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Empty constructor
        /// </summary>
        public ProcessSchedularBaseClass()
        {
            if (ReadyQueue == null)
                ReadyQueue = new List<Process>();
            if (CompletedQueue == null)
                CompletedQueue = new List<Process>();
        }
        #endregion

        #region function calls

        #region CheckProcessQueue
        /// <summary>
        /// This function will check the process queue for ready queue. This will be common to all the algorithms, except RR.
        /// <returns>a boolean value indicating whether there is any process in ready queue</returns>
        /// </summary>
        /// <remarks>Need to override it in RR.</remarks>
        public bool CheckProcessQueue()
        {
            bool _bRtnVal = false;
            ReadyQueue.RemoveAll(x => x.IsCompleted);
            _bRtnVal = ReadyQueue.Any(x => !x.IsCompleted);
            return _bRtnVal;
        }
        #endregion

        #region SortProcessQueue
        /// <summary>
        /// This function will sort the ReadyQueue. This function must be overridden in all the algorithms. 
        /// Base function must be called to show the output on console after sorting.
        /// </summary>
        public virtual void SortProcessQueue()
        {
            Console.WriteLine("Processes after sorting");
            Console.WriteLine("_________________________________________________________________");
            Console.WriteLine("Name\tArrival\tBrust\tPriority");
            for (int i = 0; i < ReadyQueue.Count; i++)
            {
                Process _tempProcess = ReadyQueue[i] as Process;
                Console.Write(_tempProcess.Name + "\t" + _tempProcess.ArrivalTime + "\t" + _tempProcess.Burst + "\t" + _tempProcess.Priority);
                Console.WriteLine();
            }
            Console.WriteLine("_________________________________________________________________");
        }
        #endregion

        #region ExecuteProcess
        /// <summary>
        /// Executes the process. This function will show the grant chart on the console screen as the execution goes on.
        /// </summary>
        protected Process ExecuteProcess(Process currentProcess)
        {
            currentProcess.WaitTime += (Counter - currentProcess.EnterTime);
            if (!IsRR)
                ProcessTotal++;
            if (!IsRR || (IsRR && currentProcess.Burst < Quanta))
            {
                if (currentProcess.Length >= currentProcess.Burst)
                {
                    Counter += currentProcess.Burst;
                    CPURunningTime += currentProcess.Burst;
                    currentProcess.Length -= currentProcess.Burst;
                    currentProcess.RunningTime += currentProcess.Burst;
                    currentProcess.ActualCPUBurst = currentProcess.Burst;
                    currentProcess.ExpectedCPUBurst = (Alpha * currentProcess.ActualCPUBurst) + ((1 - Alpha) * currentProcess.ExpectedCPUBurst);
                    currentProcess.EnterTime = Counter + IOTime;
                    if (currentProcess.Length <= 0)
                        currentProcess.IsCompleted = true;
                    currentProcess.Burst = MeanRandomNumber(CPUBurstTimes[ReadyQueue.IndexOf(currentProcess)]);
                }
                else
                {
                    Counter += currentProcess.Length;
                    CPURunningTime += currentProcess.Length;
                    currentProcess.RunningTime += currentProcess.Length;
                    currentProcess.Length = 0;
                    currentProcess.IsCompleted = true;
                    currentProcess.TurnAroundTime = currentProcess.RunningTime + currentProcess.WaitTime;
                    TotalTurnAroundTime += currentProcess.TurnAroundTime;
                    TotalWaitTime += currentProcess.WaitTime;
                }
            }
            else
            {
                CPURunningTime += Quanta;
                Counter += Quanta;
                currentProcess.Burst -= Quanta;
                currentProcess.Length -= Quanta;
                currentProcess.RunningTime += Quanta;
                currentProcess.EnterTime = Counter;
            }
            return currentProcess;
        }
        #endregion

        #region ExecuteAlgorithm
        /// <summary>
        /// This is entry point of the execution of the algorithm from outside the world.
        /// </summary>
        public virtual void ExecuteAlgorithm()
        {
        }
        #endregion

        #region DisplayFinalInformation
        /// <summary>
        /// Displays the final information such as turn around time, waiting time for each process
        /// </summary>
        public void DisplayFinalInformation()
        {
            Console.WriteLine("Name\tRunning\tWaiting\tTurnAround");
            for (int i = 0; i < CompletedQueue.Count; i++)
            {
                Console.Write("{0}\t{1}\t{2}\t{3}", CompletedQueue[i].Name, CompletedQueue[i].RunningTime / 1000, CompletedQueue[i].WaitTime / 1000, CompletedQueue[i].TurnAroundTime / 1000);
                Console.WriteLine();
            }
            Console.WriteLine("_________________________________________________________________");
            int att = 0, awt = 0;
            awt = (TotalWaitTime / CompletedQueue.Count) / 1000;
            att = (TotalTurnAroundTime / CompletedQueue.Count) / 1000;
            Console.WriteLine("A.W.T= " + awt + "\t A.T.T= " + att);
            Console.WriteLine("_________________________________________________________________");
            Console.WriteLine("CPU running time: {0} \t CPU Utilization: {1}%", (CPURunningTime / 1000), (((double)CPURunningTime / Counter) * 100).ToString("0.00"));
            Console.Write("CPU waiting time: {0} \t", (CPUWaitingTime / 1000));
            if (!IsRR)
                Console.WriteLine("Throughput: {0} Process/Sec", (((double)ProcessTotal / Counter) * 1000).ToString("0.00"));
            else
                Console.WriteLine("");
            Console.WriteLine("_________________________________________________________________");
        }
        #endregion

        #region Random time generator
        int RandomNumberGenerator()
        {
            if (Modulus.Equals(0)) return 0;
            else
            {
                Seed = (Multiplier * Seed + Increment) % Modulus;
                return Seed;
            }
        }
        int RandomNumberLimiter(int max)//return a random number, range [0,max-1]
        {
            if (max == 0) return 0;
            return RandomNumberGenerator() % max;
        }
        int MeanRandomNumber(int mean)
        {
            return RandomNumberLimiter((int)Math.Ceiling(mean * 0.2)) + (int)Math.Ceiling(mean * 0.9);
        }
        #endregion

        #region Initialize Simulation
        public void Initialize()
        {
            ContextSwitch = (Quanta * Quanta) / 200;
            Seed = DateTime.Now.Millisecond % Modulus;
            Random _randomLength = new Random();
            for (int _index = 0; _index < ReadyQueue.Count; _index++)
            {
                ReadyQueue[_index].IsStarted = true;
                ReadyQueue[_index].Burst = MeanRandomNumber(CPUBurstTimes[_index]);
                ReadyQueue[_index].Length = _randomLength.Next(2, 5) * 60 * 1000;
            }
        }
        #endregion

        #endregion
    }
}
