
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessSchedularSimulation
{
    /// <summary>
    /// Class to impletment round robin algorithm.
    /// Inherits <see cref="ProcessSchedularBaseClass"/>
    /// </summary>
    public class RR : ProcessSchedularBaseClass
    {
        #region Properties
        #endregion

        #region Constructor
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public RR() : base()
        {

        }
        #endregion

        #region Function calls

        #region SortProcessQueue
        /// <summary>
        /// Sort the base queue based on arrival time for this algorithm
        /// </summary>
        public override void SortProcessQueue()
        {
            ReadyQueue = ReadyQueue.OrderBy(x => x.ArrivalTime).ToList();
            base.SortProcessQueue();
        }
        #endregion

        #region Execute algorithm
        /// <summary>
        /// This is entry point of the execution of the algorithm from outside the world.
        /// </summary>
        public override void ExecuteAlgorithm()
        {
            Console.WriteLine("Gantt Chart");
            Console.WriteLine("_________________________________________________________________");
            double _tat = 0;
            double _wt = 0;
            while (ReadyQueue.Count > 0)
            {
                Process _tempProcess = ReadyQueue[0] as Process;
                double _originalBurst = _tempProcess.Burst;
                if (_tempProcess.RRStart.Equals(0) && _tempProcess.RRBurst.Equals(0))
                    _tempProcess.RRStart = Counter;
                if (_tempProcess.RRBurst.Equals(0))
                    _tempProcess.RRBurst = _originalBurst;
                _tempProcess.Burst = Quanta;
                ExecuteProcess(_tempProcess);
                _tempProcess.Burst = _originalBurst - Quanta;
                if (_tempProcess.Burst <= 0)
                {
                    _tempProcess.IsCompleted = true;
                    _tempProcess.Burst = _tempProcess.RRBurst;
                    _tempProcess.StartTime = _tempProcess.RRStart;
                    _tempProcess.EndTime = Counter;
                    _tempProcess.WaitTime = _tempProcess.TurnAroundTime - _tempProcess.Burst;
                    PreviousProcessEndTime = _tempProcess.EndTime;
                    _tat += _tempProcess.TurnAroundTime;
                    _wt += _tempProcess.WaitTime;
                    ReadyQueue.Remove(_tempProcess);
                    CompletedQueue.Add(_tempProcess);
                }
                else
                {
                    PreviousProcessEndTime = Counter;
                    ReadyQueue = ReadyQueue.Skip(1).Concat(ReadyQueue.Take(1)).ToList();
                }
            }
            TotalTurnAroundTime = _tat;
            TotalWaitTime = _wt;
            Console.WriteLine("_________________________________________________________________");
        }
        #endregion

        #region ToString
        /// <summary>
        /// Prints the name of the algorithm being executed.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Round Robin Scheduling";
        }
        #endregion
        #endregion
    }
}
