namespace BusTerminalWinForm
{
    public class BusTerminalConfiguration
    {
        public int BusDriveTime { get; set; } = 30;

        /// <summary>
        /// This is equal to 1/mean_arrival_rate
        /// </summary>
        public int MeanInterArrivalRate { get; set; } = 12;

        public int BoardingTime { get; set; } = 2;

        public int TotalNumberofBusStops { get; set; } = 15;

        public int TimerInterval { get; set; } = 100;

        public bool SinglePersonIncrement { get; set; } = true;

        public int MaxSimulationTime { get; set; } = 7200;
    }
}
