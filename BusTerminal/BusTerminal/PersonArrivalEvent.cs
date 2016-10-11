using LogUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTerminal
{
    public class PersonArrivalEvent : BusStopEvents
    {
        /// <summary>
        /// Average Time of person arrival on bus stop.
        /// </summary>
        private int _averageArrivalTime = 12;

        public int AverageArrivalTime
        {
            get { return _averageArrivalTime; }
            set { _averageArrivalTime = value; }
        }

        /// <summary>
        /// Creates event of type BusArrival and assigns properties to it which then needs to be added in a Queue.
        /// </summary>
        /// <param name="BusStopNumber"></param>
        /// <param name="typeOfEvent"></param>
        /// <returns></returns>
        public override BusStopEvents CreateEvent(int BusStopNumber)
        {
            PersonArrivalEvent evnt = null;
            try
            {
                evnt = new PersonArrivalEvent()
                {
                    BusStopNumber = BusStopNumber,
                    TypeofEvent = EventType.PersonArrival
                };               
                
                //Calcuate next execution time.
                evnt.TimeofExecution = ClockTime + (_averageArrivalTime * NextTimeIntervalGenerator.GetTimeInterval());
            }
            catch (Exception ex)
            {
                ApplicationLog.Instance.WriteException(ex);
            }
            return evnt;
        }

        public override BusStopEvents ExecuteEvent(BusStopEvents evnt)
        {
            try
            {
                //increase number of NumberofPersonInQueueAtStop randomly between 1 - 5 and and same has to be updated in dicNumberOfPersonsByStopNumber upon returning
                evnt.NumberofPersonInQueueAtStop = NumberofPersonInQueueAtStop + NextTimeIntervalGenerator.GetPersons(); //TODO::Randomize this
                ApplicationLog.Instance.WriteInfo(string.Format("New person(s) have arrived at stop#: {0} and total number of passnegers are {1}.", evnt.BusStopNumber, evnt.NumberofPersonInQueueAtStop));
            }
            catch (Exception ex)
            {
                ApplicationLog.Instance.WriteException(ex);
            }
            return evnt;
            
        }
    }
}
