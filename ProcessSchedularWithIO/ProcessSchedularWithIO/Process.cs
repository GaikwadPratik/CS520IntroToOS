using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessSchedularWithIO
{
    /// <summary>
    /// Represents the process instance.
    /// </summary>
    public class Process
    {
        #region Properties
        /// <summary>
        /// Name of the process
        /// </summary>
        [JsonProperty]
        public string Name { get; set; } = "P0";
        /// <summary>
        /// Indicaties the process arrival time in the process queue.
        /// </summary>
        /// <value>default value is 0.</value>
        [JsonProperty]
        public int ArrivalTime { get; set; } = 0;
        /// <summary>
        /// Indicates the CPU burst time of process.
        /// </summary>
        /// <value>default value is 0.</value>
        [JsonProperty]
        public int Burst { get; set; } = 0;
        /// <summary>
        /// Indicates the priority of the process. Lowest number indicates the higher priority.
        /// </summary>
        [JsonProperty]
        public int Priority { get; set; } = 5;
        /// <summary>
        /// Indicates the running time the process has spent in the simulation.
        /// </summary>
        /// <remarks>Not to be serialized and hence added JsonIgnore attribute</remarks>
        [JsonIgnore]
        public int RunningTime { get; set; }
        /// <summary>
        /// Indicates the wait time the process has spent in the queue.
        /// </summary>
        /// <remarks>Not to be serialized and hence added JsonIgnore attribute</remarks>
        [JsonIgnore]
        public int WaitTime { get; set; } = 0;
        /// <summary>
        /// Indicates the time when process ended its execution.
        /// </summary>
        /// <remarks>Not to be serialized and hence added JsonIgnore attribute</remarks>
        [JsonIgnore]
        public int EndTime { get; set; } = int.MaxValue;
        /// <summary>
        /// Indicates the time when process was started its execution.
        /// </summary>
        /// <remarks>Not to be serialized and hence added JsonIgnore attribute</remarks>
        [JsonIgnore]
        public int StartTime { get; set; } = 0;
        /// <summary>
        /// Indicates time spent by process in the queue since its arrival till completion
        /// </summary>
        /// <remarks>Not to be serialized and hence added JsonIgnore attribute</remarks>
        [JsonIgnore]
        public int TurnAroundTime { get; set; } = 0;
        /// <summary>
        /// Indicates whether a process is completed or not
        /// </summary>
        /// <remarks>Not to be serialized and hence added JsonIgnore attribute</remarks>
        [JsonIgnore]
        public bool IsCompleted { get; set; } = false;
        /// <summary>
        /// Indicates whether a process is started or not
        /// </summary>
        /// <remarks>Not to be serialized and hence added JsonIgnore attribute</remarks>
        [JsonIgnore]
        public bool IsStarted { get; set; } = false;
        /// <summary>
        /// Burst parameter used for RR only. Other algorithms can ignore this.
        /// </summary>
        /// <remarks>Not to be serialized and hence added JsonIgnore attribute</remarks>
        [JsonIgnore]
        public int RRBurst { get; set; } = 0;
        /// <summary>
        /// Start parameter used for RR only. Other algorithms can ignore this.
        /// </summary>
        /// <remarks>Not to be serialized and hence added JsonIgnore attribute</remarks>
        [JsonIgnore]
        public int RRStart { get; set; } = 0;
        /// <summary>
        /// Ideal CPU burst, this is expected time at which CPU Burst should occur without IO wait.
        /// </summary>
        [JsonIgnore]
        public int ExpectedCPUBurst { get; set; } = 0;
        /// <summary>
        /// Actual CPU burst, this is time at which CPU Burst occurs with IO wait.
        /// </summary>
        [JsonIgnore]
        public int ActualCPUBurst { get; set; } = 0;
        /// <summary>
        /// Length for which process should run 
        /// </summary>
        public int Length { get; set; } = 3 * 60 * 1000;
        /// <summary>
        /// Time at which process enters in ready queue
        /// </summary>
        [JsonIgnore]
        public int EnterTime { get; set; } = 0;
        #endregion

        #region Constructor
        /// <summary>
        /// Empty constructor
        /// </summary>
        public Process()
        {

        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arrivalTime"></param>
        /// <param name="brust"></param>
        /// <param name="priority"></param>
        public Process(string name, int arrivalTime, int brust, int priority)
        {
            Name = name;
            ArrivalTime = arrivalTime;
            Burst = brust;
            Priority = priority;
        }
        #endregion
    }
}
