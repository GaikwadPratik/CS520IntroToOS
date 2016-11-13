using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcessSchedularSimulation
{
    /// <summary>
    /// Base class to all the algorithms in the process scheduling. All the algorithms must inherit this class.
    /// The objects must of this class but instantiation should be done for child classes.
    /// </summary>
    public class ProcessSchedularBaseClass
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
        public double Counter { get; set; } = 0;

        /// <summary>
        /// Just to pass the end time of the previous process
        /// </summary>
        public double PreviousProcessEndTime { get; set; }

        /// <summary>
        /// Total wait time of the queue.
        /// </summary>
        public double TotalWaitTime { get; set; }

        /// <summary>
        /// Total turn around time of the queue.
        /// </summary>
        public double TotalTurnAroundTime { get; set; }

        /// <summary>
        /// Time quanta required for Round robin. For all other this can be ignored.
        /// </summary>
        public int Quanta { get; set; }
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
        protected void ExecuteProcess(Process executingProcess)
        {
            Console.Write(executingProcess.Name + "\t");
            if (executingProcess.ArrivalTime < Counter)
            {
                PrintSpaces(Counter);
                executingProcess.StartTime = executingProcess.ArrivalTime;
                //Counter += executingProcess.StartTime = executingProcess.ArrivalTime;
                if (PreviousProcessEndTime > 0)
                    executingProcess.StartTime = PreviousProcessEndTime;
            }
            else
            {
                PrintSpaces(executingProcess.ArrivalTime);
                //Counter = executingProcess.ArrivalTime;
            }

            if (executingProcess.StartTime > executingProcess.ArrivalTime)
                executingProcess.WaitTime = executingProcess.StartTime - executingProcess.ArrivalTime;
            else
                executingProcess.WaitTime = 0;

            executingProcess.EndTime = executingProcess.StartTime + executingProcess.Burst;
            executingProcess.TurnAroundTime = executingProcess.WaitTime + executingProcess.Burst;

            //TODO:: check below two lines for RR and move or avoid them if necessary
            TotalWaitTime += executingProcess.WaitTime;
            TotalTurnAroundTime += executingProcess.TurnAroundTime;

            PrintHashes(executingProcess.Burst);
            Counter += executingProcess.Burst;
            Console.WriteLine();
        }
        #endregion

        /// <summary>
        /// This is entry point of the execution of the algorithm from outside the world.
        /// </summary>
        public virtual void ExecuteAlgorithm()
        {
        }

        #region PrintSpaces
        /// <summary>
        /// This function will print spaces to new process with respect to old process so that contination
        /// with respect to time is maintained.
        /// </summary>
        /// <param name="counter"></param>
        private void PrintSpaces(double counter)
        {
            for (int i = 0; i < counter; i++)
            {
                Console.Write(" ");
            }
        }
        #endregion

        #region PrintHashes
        /// <summary>
        /// This function will print hashes to indicates the exeuction progress of process
        /// </summary>
        /// <param name="brust"></param>
        private void PrintHashes(double brust)
        {
            for (int i = 0; i < brust; i++)
            {
                Console.Write("#");
            }
        }
        #endregion

        #region DisplayFinalInformation
        /// <summary>
        /// Displays the final information such as turn around time, waiting time for each process
        /// </summary>
        public void DisplayFinalInformation()
        {
            //int clock = 0, totalwait = 0, totalturnAround = 0;
            //for (int i = 0; i < CompletedQueue.Count; i++)
            //{
            //    if (CompletedQueue[i].ArrivalTime > clock)
            //    {
            //        CompletedQueue[i].StartTime = CompletedQueue[i].ArrivalTime;
            //        clock += CompletedQueue[i].StartTime - CompletedQueue[i].ArrivalTime;
            //        clock += CompletedQueue[i].Brust;
            //    }
            //    else
            //    {
            //        if (i > 0)
            //            CompletedQueue[i].StartTime = CompletedQueue[i - 1].EndTime;
            //        clock += CompletedQueue[i].Brust;
            //    }
            //    if (CompletedQueue[i].StartTime > CompletedQueue[i].ArrivalTime)
            //        CompletedQueue[i].WaitTime = CompletedQueue[i].StartTime - CompletedQueue[i].ArrivalTime;
            //    else
            //        CompletedQueue[i].WaitTime = 0;
            //    CompletedQueue[i].EndTime = CompletedQueue[i].StartTime + CompletedQueue[i].Brust;
            //    CompletedQueue[i].TurnAroundTime = CompletedQueue[i].WaitTime + CompletedQueue[i].Brust;
            //    totalwait += CompletedQueue[i].WaitTime;
            //    totalturnAround += CompletedQueue[i].TurnAroundTime;
            //}

            Console.WriteLine("Name\tArrival\tBrust\tStart\tEnd\tWait\tturnaround");
            for (int i = 0; i < CompletedQueue.Count; i++)
            {
                Console.Write("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", CompletedQueue[i].Name, CompletedQueue[i].ArrivalTime, CompletedQueue[i].Burst, CompletedQueue[i].StartTime, CompletedQueue[i].EndTime, CompletedQueue[i].WaitTime, CompletedQueue[i].TurnAroundTime);
                Console.WriteLine();
            }
            Console.WriteLine("_________________________________________________________________");
            double att = 0, awt = 0;
            awt = TotalWaitTime / CompletedQueue.Count;
            att = TotalTurnAroundTime / CompletedQueue.Count;
            Console.WriteLine("A.W.T= " + awt + "\t A.T.T= " + att);
            Console.WriteLine("_________________________________________________________________");
        }
        #endregion

        #endregion
    }
}
