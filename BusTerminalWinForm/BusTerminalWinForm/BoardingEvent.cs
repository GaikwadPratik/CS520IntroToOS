using LogUtils;
using System;

namespace BusTerminalWinForm
{
    public class BoardingEvent : BusStopEvents
    {
        public int BusNumber { get; set; }

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
                    BusNumber = this.BusNumber
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

        public override BusStopEvents ExecuteEvent()
        {
            BoardingEvent _event = null;
            try
            {
                _event = this;
                ApplicationLog.Instance.WriteInfo(string.Format("Boarding passenger in Bus# {0} at stop# {1}.", BusNumber, _event.BusStopNumber));
                _event.NumberofPersonInQueueAtStop -= 1;
            }
            catch (Exception ex)
            {
                ApplicationLog.Instance.WriteException(ex);
            }
            return _event;
        }
    }
}
