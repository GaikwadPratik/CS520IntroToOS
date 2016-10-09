using LogUtils;
using System;

namespace BusTerminalWinForm
{
    public class BusStopEvents
    {

        private int _numberOfPassangersAtBusStop = 0;

        /// <summary>
        /// Clock time indicating at which event is occurring
        /// </summary>
        private int _clockTime = 0;

        /// <summary>
        /// Time for a person to board the bus
        /// </summary>
        private int _boardingTime = 2;

        /// <summary>
        /// Bus stop at which event is occurring
        /// </summary>
        public int BusStopNumber { get; set; }

        /// <summary>
        /// Type of the event which is occurring
        /// </summary>
        public EventType TypeofEvent { get; set; }

        /// <summary>
        /// Time at which an event should occur
        /// </summary>
        public double TimeofExecution { get; set; }

        public int BoardingTime
        {
            get { return _boardingTime; }
            set { _boardingTime = value; }
        }

        public int ClockTime
        {
            get { return _clockTime; }
            set { _clockTime = value; }
        }

        /// <summary>
        /// number of persons in a queue at any bus stop
        /// </summary>
        public int NumberofPersonInQueueAtStop
        {
            get { return _numberOfPassangersAtBusStop; }
            set { _numberOfPassangersAtBusStop = value; }
        }

        /// <summary>
        /// Creates a base event on which child properties will be created from child classes.
        /// </summary>
        /// <param name="BusStopNumber"></param>
        /// <param name="eventType"></param>
        /// <returns></returns>
        public virtual BusStopEvents CreateEvent(int BusStopNumber)
        {
            BusStopEvents evnt = null;
            try
            {
                evnt = new BusStopEvents()
                {
                    BusStopNumber = BusStopNumber
                };
            }
            catch (Exception ex)
            {
                ApplicationLog.Instance.WriteException(ex);
            }
            return evnt;
        }

        public virtual BusStopEvents ExecuteEvent()
        {
            BusStopEvents _busStopevent = null;
            try
            {
                _busStopevent = new BusStopEvents()
                {
                    BusStopNumber = BusStopNumber
                };
            }
            catch (Exception ex)
            {
                ApplicationLog.Instance.WriteException(ex);
            }
            return _busStopevent;
        }
    }
}
