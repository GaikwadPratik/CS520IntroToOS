﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessSchedularSimulation
{
    /// <summary>
    /// Class to impletment First come first server algorithm.
    /// Inherits <see cref="ProcessSchedularBaseClass"/>
    /// </summary>
    public class FCFS : ProcessSchedularBaseClass
    {
        #region Properties
        #endregion

        #region Constructor
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public FCFS() : base()
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
            foreach (Process _tempProcess in ReadyQueue)
            {
                ExecuteProcess(_tempProcess);
                _tempProcess.IsCompleted = true;
                CompletedQueue.Add(_tempProcess);
                PreviousProcessEndTime = _tempProcess.EndTime;
            }
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
            return "First Come First Serve Scheduling";
        }
        #endregion

        #endregion
    }
}
