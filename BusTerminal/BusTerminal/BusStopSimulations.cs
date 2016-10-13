using LogUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTerminal
{
    public class BusStopSimulation
    {
        public BusTerminalConfiguration Configuration { get; set; }
        Dictionary<int, int> _dicNumberOfPersonsByStopNumber = new Dictionary<int, int>();

        /// <summary>
        /// List of events to be executed. This list will be modified after occurence of each event
        /// </summary>
        private List<BusStopEvents> _lstBusStopevents = new List<BusStopEvents>();

        public void SimulateEvents()
        {
            NextTimeIntervalGenerator.SinglePersonIncrement = Configuration.SinglePersonIncrement;
            int _simulationTime = Configuration.MaxSimulationTime;
            ApplicationLog.Instance.WriteInfo(string.Format("Simulation started for {0} seconds.", _simulationTime));
            if (_lstBusStopevents != null && _lstBusStopevents.Count > 0)
            {
                int _clockTime = 1;
                double _boardinTimeExecution = 0;
                do
                {
                    BusStopEvents _event = _lstBusStopevents[0];
                    BusStopEvents _nextEvent = null;
                    if (_event != null)
                    {
                        int _currentPassangersAtStop = 0;
                        if (_dicNumberOfPersonsByStopNumber.Keys.Contains(_event.BusStopNumber))
                            _dicNumberOfPersonsByStopNumber.TryGetValue(_event.BusStopNumber, out _currentPassangersAtStop);
                        int _eventBusStopNumber = _event.BusStopNumber;
                        _event.NumberofPersonInQueueAtStop = _currentPassangersAtStop;
                        _lstBusStopevents.Remove(_event);
                        switch (_event.TypeofEvent)
                        {
                            #region EventType.PersonArrival
                            case EventType.PersonArrival:
                                //Execute the current event
                                _event = _event.ExecuteEvent(_event);
                                _dicNumberOfPersonsByStopNumber[_eventBusStopNumber] = _event.NumberofPersonInQueueAtStop;
                                //Create Person Arrival event for next person.
                                _nextEvent = new PersonArrivalEvent()
                                {
                                    NumberofPersonInQueueAtStop = _currentPassangersAtStop,
                                    ClockTime = _clockTime,
                                    AverageArrivalTime = Configuration.MeanInterArrivalRate
                                };
                                _nextEvent = _nextEvent.CreateEvent(_eventBusStopNumber) as PersonArrivalEvent;
                                break;
                            #endregion

                            #region EventType.BusArrial
                            case EventType.BusArrial:
                                //TODO:: While creating a new Bus event, read the lstEventsToExecute.                                
                                IEnumerable<BusArrivalEvent> _lstSameTimeBusses = _lstBusStopevents.Where(x => x.TypeofEvent.Equals(EventType.BusArrial) && x.TimeofExecution.Equals(_event.TimeofExecution) && x.BusStopNumber.Equals(_event.BusStopNumber)) as IEnumerable<BusArrivalEvent>;
                                if (_lstSameTimeBusses != null && _lstSameTimeBusses.Any())
                                {
                                    IEnumerable<int> _lstWaitingBusNumbers = _lstSameTimeBusses.Select(x => x.BusNumber);
                                    string sBusNumbers = string.Empty;
                                    if (_lstWaitingBusNumbers != null && _lstWaitingBusNumbers.Any())
                                        foreach (int nbusnum in _lstWaitingBusNumbers)
                                            sBusNumbers = string.Format("{0},{1}", sBusNumbers, nbusnum);
                                    if (!string.IsNullOrEmpty(sBusNumbers))
                                        ApplicationLog.Instance.WriteInfo(string.Format("Busesses {0} are waiting at {1} becuase another bus is already boarding passengers.", sBusNumbers, _event.BusStopNumber));
                                }

                                _event = _event.ExecuteEvent(_event);
                                _nextEvent = new BusArrivalEvent();

                                //Bus arrival for next bus stop.
                                _nextEvent = _event.CreateEvent(_eventBusStopNumber + 1);
                                _nextEvent.NumberofPersonInQueueAtStop = _currentPassangersAtStop;
                                _nextEvent.ClockTime = _clockTime;
                                _boardinTimeExecution = _nextEvent.TimeofExecution;
                                break;
                            #endregion

                            #region EventType.PersonBoarding
                            case EventType.PersonBoarding:

                                _event = _event.ExecuteEvent(_event);

                                //Make passengers at bus stop 0 since they are all boarded.
                                _dicNumberOfPersonsByStopNumber[_eventBusStopNumber] = 0;
                                _event.TimeofExecution = _boardinTimeExecution;
                                _boardinTimeExecution = 0;
                                //Boarding event for next bus stop
                                _nextEvent = _event.CreateEvent(_eventBusStopNumber + 1);

                                //if (_event.NumberofPersonInQueueAtStop != 0)
                                //{
                                //	_event = _event.ExecuteEvent(_event);

                                //	//Make passengers at bus stop 0 since they are all boarded.
                                //	_dicNumberOfPersonsByStopNumber[_eventBusStopNumber] = _event.NumberofPersonInQueueAtStop;
                                //	_event.TimeofExecution = _clockTime + _event.BoardingTime;
                                //	//Boarding event for next bus stop
                                //	_nextEvent = _event.CreateEvent(_eventBusStopNumber);
                                //}
                                //else
                                //{
                                //	//Make passengers at bus stop 0 since they are all boarded.
                                //	_dicNumberOfPersonsByStopNumber[_eventBusStopNumber] = 0;
                                //	_event.TimeofExecution = _boardinTimeExecution;
                                //	_boardinTimeExecution = 0;
                                //	//Boarding event for next bus stop
                                //	_nextEvent = _event.CreateEvent(_eventBusStopNumber + 1);
                                //}
                                break;
                                #endregion
                        }
						if(_nextEvent != null)
							AddToQueue(_nextEvent);
                    }
                    Console.WriteLine(_simulationTime);
                    _simulationTime -= 1;
                    _clockTime += 1;
                }
                while (_simulationTime > 0);

                ApplicationLog.Instance.WriteInfo(string.Format("Simulation ended after {0} seconds.", _simulationTime));
            }
            else
                ApplicationLog.Instance.WriteError("Stopping simulation due to in appropriate initialization");
        }

        public void Initialize()
        {
            int _totalBusStops = Configuration.TotalNumberofBusStops;
            int _busNumber = 1;
            for (int _nIndex = 1; _nIndex <= _totalBusStops; _nIndex++)
            {

                int _currentPassangersAtStop = 0;
                if (_dicNumberOfPersonsByStopNumber.Keys.Contains(_nIndex))
                    _dicNumberOfPersonsByStopNumber.TryGetValue(_nIndex, out _currentPassangersAtStop);

                if ((_nIndex % 3).Equals(0))
                {
                    //Bus arrival event
                    BusArrivalEvent _busArrivalEvent = new BusArrivalEvent()
                    {
                        NumberofPersonInQueueAtStop = _currentPassangersAtStop,
                        BusNumber = _busNumber,
                        ClockTime = _nIndex,
                        DriveTime = Configuration.BusDriveTime,
                        BoardingTime = Configuration.BoardingTime
                    };

                    _busArrivalEvent = _busArrivalEvent.CreateEvent(_nIndex) as BusArrivalEvent;

                    AddToQueue(_busArrivalEvent);

                    BoardingEvent _boardingEvent = new BoardingEvent()
                    {
                        NumberofPersonInQueueAtStop = _currentPassangersAtStop,
                        BusNumber = _busNumber,
                        TimeofExecution = _busArrivalEvent.TimeofExecution,
                        BoardingTime = Configuration.BoardingTime
                    };

                    _boardingEvent = _boardingEvent.CreateEvent(_nIndex) as BoardingEvent;

                    AddToQueue(_boardingEvent);
                    _busNumber += 1;
                }
                //Person Arrival event.
                PersonArrivalEvent _personArrival = new PersonArrivalEvent()
                {
                    NumberofPersonInQueueAtStop = _currentPassangersAtStop,
                    ClockTime = _nIndex,
                    AverageArrivalTime = Configuration.MeanInterArrivalRate
                };
                _personArrival = _personArrival.CreateEvent(_nIndex) as PersonArrivalEvent;
                AddToQueue(_personArrival);
                _dicNumberOfPersonsByStopNumber.Add(_nIndex, _personArrival.NumberofPersonInQueueAtStop);
            }
        }

        private void AddToQueue(BusStopEvents evnt)
        {
            try
            {
                _lstBusStopevents.Add(evnt);
                _lstBusStopevents = _lstBusStopevents.OrderBy(x => x.TimeofExecution).ToList();
            }
            catch (Exception ex)
            {
                ApplicationLog.Instance.WriteException(ex);
            }
        }
    }

    public enum EventType
    {
        PersonArrival,
        BusArrial,
        PersonBoarding
    }
}
