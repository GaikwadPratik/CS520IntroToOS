﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessSchedularWithIO
{
    /// <summary>
    /// Class to impletment Shortest job first algorithm.
    /// Inherits <see cref="ProcessSchedularBaseClass"/>
    /// </summary>
    class SJF : ProcessSchedularBaseClass
    {
        #region Properties
        #endregion

        #region Constructor
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public SJF() : base()
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
            ReadyQueue = ReadyQueue.OrderBy(x => x.Burst).ToList();
            base.SortProcessQueue();
        }
        #endregion

        #region Execute algorithm
        /// <summary>
        /// This is entry point of the execution of the algorithm from outside the world.
        /// </summary>
        public override void ExecuteAlgorithm()
        {
            Console.WriteLine("_________________________________________________________________");

            while (true)
            {
                int _tempProcessId = -1;
                int _shortest = int.MaxValue;
                bool _isDone = ReadyQueue.All(x => x.IsCompleted);
                for (int _index = 0; _index < ReadyQueue.Count; _index++)
                {
                    if (ReadyQueue[_index].IsStarted && !ReadyQueue[_index].IsCompleted && ReadyQueue[_index].ExpectedCPUBurst < _shortest && ReadyQueue[_index].EnterTime < Counter)
                    {
                        _tempProcessId = _index;
                        _shortest = ReadyQueue[_index].Burst;
                    }
                }

                if (_tempProcessId >= 0)
                {
                    Process _executedProcess = ExecuteProcess(ReadyQueue[_tempProcessId]);
                    if (_executedProcess.IsCompleted)
                        CompletedQueue.Add(_executedProcess);
                }
                else
                {
                    if (!_isDone)
                    {
                        Counter += 5;
                        CPUWaitingTime += 5;
                    }
                    else
                        break;
                }
            }

            CompletedQueue = CompletedQueue.OrderBy(x => x.Name).ToList();
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
            return "Shortest Job First Scheduling";
        }
        #endregion
        #endregion
    }
}
