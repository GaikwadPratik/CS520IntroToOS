using LogUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTerminal
{
    public class BusArrivalEvent : BusStopEvents
    {
        /// <summary>
        /// Drive time between two bus stops
        /// </summary>
        private int _driveTime = 300;

        /// <summary>
        /// Bus number between 1-5
        /// </summary>
        public int BusNumber { get; set; }

        public int DriveTime
        {
            get { return _driveTime; }
        }

        /// <summary>
        /// Creates event of type BusArrival and assigns properties to it which then needs to be added in a Queue.
        /// </summary>
        /// <param name="BusStopNumber"></param>
        /// <param name="typeOfEvent"></param>
        /// <returns></returns>
        public override BusStopEvents CreateEvent(int busStopNumber)
        {
            BusArrivalEvent evnt = null;
            try
            {
                if (busStopNumber.Equals(16))
                    busStopNumber = 1;
                evnt = new BusArrivalEvent()
                {
                    BusStopNumber = busStopNumber,
                    TypeofEvent = EventType.BusArrial,
                    BusNumber = this.BusNumber,
                    ClockTime = this.ClockTime
                };
                //Calcuate execution time for next bus stop.
                evnt.TimeofExecution = ClockTime + _driveTime + NumberofPersonInQueueAtStop * BoardingTime;
            }
            catch (Exception ex)
            {
                ApplicationLog.Instance.WriteException(ex);
            }
            return evnt;
        }

        public override BusStopEvents ExecuteEvent(BusStopEvents evnt)
        {
            //Todo:: check if any other bus is coming to bus stop and mark it as waiting. How to handle with 0 passanger and no overtaking?
            BusArrivalEvent _event = null;
            try
            {
                _event = evnt as BusArrivalEvent;
                ApplicationLog.Instance.WriteInfo(string.Format("Bus# {0} arrived at stop# {1}.", _event.BusNumber, _event.BusStopNumber));
            }
            catch (Exception ex)
            {
                ApplicationLog.Instance.WriteException(ex);
            }
            return _event;
        }
    }

}
