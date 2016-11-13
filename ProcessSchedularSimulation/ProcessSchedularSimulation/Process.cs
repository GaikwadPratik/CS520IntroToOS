using Newtonsoft.Json;

namespace ProcessSchedularSimulation
{
    /// <summary>
    /// Represents the process instance.
    /// </summary>
    public class Process
    {
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
        public double ArrivalTime { get; set; } = 0;
        /// <summary>
        /// Indicates the CPU burst time of process.
        /// </summary>
        /// <value>default value is 0.</value>
        [JsonProperty]
        public double Burst { get; set; } = 0;
        /// <summary>
        /// Indicates the priority of the process. Lowest number indicates the higher priority.
        /// </summary>
        [JsonProperty]
        public int Priority { get; set; } = 5;
        /// <summary>
        /// Indicates the wait time the process has spent in the queue.
        /// </summary>
        /// <remarks>Not to be serialized and hence added JsonIgnore attribute</remarks>
        [JsonIgnore]
        public double WaitTime { get; set; } = 0;
        /// <summary>
        /// Indicates the time when process ended its execution.
        /// </summary>
        /// <remarks>Not to be serialized and hence added JsonIgnore attribute</remarks>
        [JsonIgnore]
        public double EndTime { get; set; } = int.MaxValue;
        /// <summary>
        /// Indicates the time when process was started its execution.
        /// </summary>
        /// <remarks>Not to be serialized and hence added JsonIgnore attribute</remarks>
        [JsonIgnore]
        public double StartTime { get; set; } = 0;
        /// <summary>
        /// Indicates time spent by process in the queue since its arrival till completion
        /// </summary>
        /// <remarks>Not to be serialized and hence added JsonIgnore attribute</remarks>
        [JsonIgnore]
        public double TurnAroundTime { get; set; } = 0;
        /// <summary>
        /// Indicates whether a process is completed or not
        /// </summary>
        /// <remarks>Not to be serialized and hence added JsonIgnore attribute</remarks>
        [JsonIgnore]
        public bool IsCompleted { get; set; } = false;

        /// <summary>
        /// Burst parameter used for RR only. Other algorithms can ignore this.
        /// </summary>
        /// <remarks>Not to be serialized and hence added JsonIgnore attribute</remarks>
        public double RRBurst { get; set; } = 0;

        /// <summary>
        /// Start parameter used for RR only. Other algorithms can ignore this.
        /// </summary>
        /// <remarks>Not to be serialized and hence added JsonIgnore attribute</remarks>
        public double RRStart { get; set; } = 0;

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
    }
}
