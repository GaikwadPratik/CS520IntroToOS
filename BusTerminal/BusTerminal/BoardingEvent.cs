using LogUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTerminal
{
    public class BoardingEvent : BusStopEvents
    {
        public int BusNumber { get; set; }

        public override BusStopEvents ExecuteEvent(BusStopEvents evnt)
        {
            BoardingEvent _event = null;
            try
            {
                _event = evnt as BoardingEvent;
                ApplicationLog.Instance.WriteInfo(string.Format("There are {0} passengers at stop# {1} and hence bus will be at stop for {2}.", _event.NumberofPersonInQueueAtStop, _event.BusStopNumber, BoardingTime * _event.NumberofPersonInQueueAtStop));
                do
                {
                    ApplicationLog.Instance.WriteInfo(string.Format("Boarding passenger in Bus# {0} at stop# {1}.", BusNumber, _event.BusStopNumber));
                    _event.NumberofPersonInQueueAtStop -= 1;
                }
                while (_event.NumberofPersonInQueueAtStop > 0);
                ApplicationLog.Instance.WriteInfo(string.Format("All passengers have boarded Bus# {0} at stop# {1}.", BusNumber, _event.BusStopNumber));
            }
            catch (Exception ex)
            {
                ApplicationLog.Instance.WriteException(ex);
            }
            return _event;
        }

        public override BusStopEvents CreateEvent(int busStopNumber)
        {
            BoardingEvent _evnt = null;
            try
            {
                if (busStopNumber.Equals(16))
                    busStopNumber = 1;
                _evnt = new BoardingEvent()
                {
                    BusStopNumber = busStopNumber,
                    TypeofEvent = EventType.PersonBoarding,
                    BusNumber = this.BusNumber,
                    BoardingTime = this.BoardingTime
                };

                //Calcuate next execution time.
                _evnt.TimeofExecution = this.TimeofExecution; //ClockTime + _driveTime + NumberofPersonInQueueAtStop * BoardingTime
            }
            catch (Exception ex)
            {
                ApplicationLog.Instance.WriteException(ex);
            }
            return _evnt;
        }
    }
}
