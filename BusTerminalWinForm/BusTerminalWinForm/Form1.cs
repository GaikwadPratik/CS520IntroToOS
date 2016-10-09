using LogUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace BusTerminalWinForm
{
    public partial class Form1 : Form
    {
        BusTerminalConfiguration _busTerminalConfiguration = null;
        int _simulationTime = 7200;
        int _timerCounter = 0;
        //Label _lblEventOccuranceLocation = null;
        Label _lblPassengerCount = null;
        Label _lblTimerValue = null;
        TextBox _txtStop = null;
        double _boardinTimeExecution = 0;
        int _ntotalBuses = 1;

        /// <summary>
        /// Dictionary of bus stop numbers and number of passangers at those bus stop
        /// </summary>
        Dictionary<int, int> _dicNumberOfPersonsByStopNumber = null;

        /// <summary>
        /// List of events to be executed. This list will be modified after occurence of each event
        /// </summary>
        private List<BusStopEvents> _lstBusStopevents = null;

        public Form1()
        {
            InitializeComponent();

            _busTerminalConfiguration = new BusTerminalConfiguration();
            NextTimeIntervalGenerator.InternalConfiguration = _busTerminalConfiguration;
            _dicNumberOfPersonsByStopNumber = new Dictionary<int, int>();
            _lstBusStopevents = new List<BusStopEvents>();
            timer1.Interval = _busTerminalConfiguration.TimerInterval;
            _simulationTime = _busTerminalConfiguration.MaxSimulationTime;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Initialize();
            ApplicationLog.Instance.WriteInfo(string.Format("Simulation started for {0} seconds.", _simulationTime));

            chartBusArrival.ChartAreas[0].AxisX.Minimum = 0;
            chartPassangerQueue.ChartAreas[0].AxisX.Minimum = 0;

            int _seriesCount = 1;
            do
            {
                chartBusArrival.Series.Add(string.Format("BusNumber{0}", _seriesCount));
                chartBusArrival.Series[string.Format("BusNumber{0}", _seriesCount)].ChartType = SeriesChartType.Line;
                chartBusArrival.Series[string.Format("BusNumber{0}", _seriesCount)].BorderWidth = 5;
                chartBusArrival.Series[string.Format("BusNumber{0}", _seriesCount)].BorderDashStyle = ChartDashStyle.DashDot;

                _seriesCount += 1;
            }
            while (_seriesCount <= _ntotalBuses);

            for (int index = 1; index <= _busTerminalConfiguration.TotalNumberofBusStops; index++)
            {
                chartPassangerQueue.Series.Add(string.Format("StopNumber{0}", index));
                chartPassangerQueue.Series[string.Format("StopNumber{0}", index)].ChartType = SeriesChartType.Column;
            }
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTimerValue.Text = _timerCounter.ToString();
            SimulateEvents();
        }

        private void PlotPassengerQueue()
        {
            if (_timerCounter < 1000 && _timerCounter % 60 == 0)
            {
                foreach (KeyValuePair<int, int> kvp in _dicNumberOfPersonsByStopNumber)
                {
                    chartPassangerQueue.Series[string.Format("StopNumber{0}", kvp.Key)].Points.AddXY(_timerCounter, kvp.Value);
                }
            }
        }

        /// <summary>
        /// Simulation will begin here.
        /// </summary>
        public void SimulateEvents()
        {
            if (_lstBusStopevents != null && _lstBusStopevents.Count > 0)
            {

                BusStopEvents _event = _lstBusStopevents[0];
                BusStopEvents _nextEvent = null;
                if (_event != null)
                {
                    int _eventBusStopNumber = _event.BusStopNumber;
                    _txtStop = Controls.Find(string.Format("txtStop{0}", _event.BusStopNumber), true).FirstOrDefault() as TextBox;
                    _lblPassengerCount = Controls.Find(string.Format("lblPassengerCount{0}", _event.BusStopNumber), true).FirstOrDefault() as Label;

                    int _currentPassangersAtStop = 0;
                    int.TryParse(_lblPassengerCount.Text, out _currentPassangersAtStop);

                    _txtStop.Text = _event.TypeofEvent.ToString();

                    _event.NumberofPersonInQueueAtStop = _currentPassangersAtStop;
                    _lstBusStopevents.Remove(_event);

                    switch (_event.TypeofEvent)
                    {
                        #region EventType.PersonArrival
                        case EventType.PersonArrival:
                            //Execute the current event
                            _event = _event.ExecuteEvent();
                            _lblPassengerCount.Text = _event.NumberofPersonInQueueAtStop.ToString();
                            //Create Person Arrival event for next person.
                            _nextEvent = new PersonArrivalEvent()
                            {
                                NumberofPersonInQueueAtStop = _currentPassangersAtStop,
                                ClockTime = _timerCounter
                            };
                            _nextEvent = _nextEvent.CreateEvent(_eventBusStopNumber) as PersonArrivalEvent;
                            _dicNumberOfPersonsByStopNumber[_eventBusStopNumber] = _event.NumberofPersonInQueueAtStop;
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

                            _event = _event.ExecuteEvent();
                            _nextEvent = new BusArrivalEvent();

                            //Bus arrival for next bus stop.
                            _nextEvent = _event.CreateEvent(_eventBusStopNumber + 1);
                            _nextEvent.NumberofPersonInQueueAtStop = _currentPassangersAtStop;
                            _nextEvent.ClockTime = _timerCounter;
                            _boardinTimeExecution = _nextEvent.TimeofExecution;
                            BusArrivalEvent _event1 = _event as BusArrivalEvent;
                            chartBusArrival.Series[string.Format("BusNumber{0}", _event1.BusNumber)].Points.AddXY(_timerCounter, _event.BusStopNumber);

                            break;
                        #endregion

                        #region EventType.PersonBoarding
                        case EventType.PersonBoarding:
                            timer1.Stop();

                            //int _nStoredPassngers = 0;
                            //_dicNumberOfPersonsByStopNumber.TryGetValue(_eventBusStopNumber, out _nStoredPassngers);
                            //if (_nStoredPassngers < _event.NumberofPersonInQueueAtStop)
                            //    _dicNumberOfPersonsByStopNumber[_eventBusStopNumber] = _event.NumberofPersonInQueueAtStop;

                            ApplicationLog.Instance.WriteInfo(string.Format("There are {0} passengers at stop# {1} and hence bus will be at stop for {2}.", _event.NumberofPersonInQueueAtStop, _event.BusStopNumber, _event.BoardingTime * _event.NumberofPersonInQueueAtStop));
                            timer2.Tick += (sender, e) => timer2_Tick(sender, e, _event, _lblPassengerCount);
                            timer2.Interval = _busTerminalConfiguration.TimerInterval;
                            timer2.Start();
                            //Make passengers at bus stop 0 since they are all boarded.                            
                            _event.TimeofExecution = _boardinTimeExecution;
                            _boardinTimeExecution = 0;
                            //Boarding event for next bus stop
                            _nextEvent = _event.CreateEvent(_eventBusStopNumber + 1);

                            break;
                            #endregion
                    }
                    AddToQueue(_nextEvent);
                }

                CheckMainCounterStopCondition();
            }
            else
                ApplicationLog.Instance.WriteError("Stopping simulation due to in appropriate initialization");
        }

        private void CheckMainCounterStopCondition()
        {
            PlotPassengerQueue();

            if (_timerCounter.Equals(_simulationTime))
            {
                timer1.Stop();
                ApplicationLog.Instance.WriteInfo(string.Format("Simulation ended after {0} seconds.", _simulationTime));
            }
            else
                _timerCounter += 1;
        }

        public void Initialize()
        {
            int _totalBusStops = _busTerminalConfiguration.TotalNumberofBusStops;
            int _busNumber = 1;
            for (int _nIndex = 1; _nIndex <= _totalBusStops; _nIndex++)
            {
                _lblPassengerCount = Controls.Find(string.Format("lblPassengerCount{0}", _nIndex), true).FirstOrDefault() as Label;
                int _currentPassangersAtStop = 0;
                int.TryParse(_lblPassengerCount.Text, out _currentPassangersAtStop);

                if ((_nIndex % 3).Equals(0))
                {
                    //Bus arrival event
                    BusArrivalEvent _busArrivalEvent = new BusArrivalEvent()
                    {
                        NumberofPersonInQueueAtStop = _currentPassangersAtStop,
                        BusNumber = _busNumber,
                        ClockTime = _nIndex
                    };

                    _busArrivalEvent = _busArrivalEvent.CreateEvent(_nIndex) as BusArrivalEvent;

                    AddToQueue(_busArrivalEvent);

                    BoardingEvent _boardingEvent = new BoardingEvent()
                    {
                        NumberofPersonInQueueAtStop = _currentPassangersAtStop,
                        BusNumber = _busNumber,
                        TimeofExecution = _busArrivalEvent.TimeofExecution
                    };

                    _boardingEvent = _boardingEvent.CreateEvent(_nIndex) as BoardingEvent;

                    AddToQueue(_boardingEvent);
                    _busNumber += 1;
                }
                //Person Arrival event.
                PersonArrivalEvent _personArrival = new PersonArrivalEvent()
                {
                    NumberofPersonInQueueAtStop = _currentPassangersAtStop,
                    ClockTime = _nIndex
                };
                _personArrival = _personArrival.CreateEvent(_nIndex) as PersonArrivalEvent;
                AddToQueue(_personArrival);
                _lblPassengerCount.Text = _personArrival.NumberofPersonInQueueAtStop.ToString();
            }
            _ntotalBuses = _busNumber;
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

        private void timer2_Tick(object sender, EventArgs e, BusStopEvents _boardinEvent, Label _lblPassengerCount)
        {
            lblTimerValue.Text = _timerCounter.ToString();
            _boardinEvent = _boardinEvent.ExecuteEvent();
            _lblPassengerCount.Text = _boardinEvent.NumberofPersonInQueueAtStop.ToString();
            CheckMainCounterStopCondition();
            PlotPassengerQueue();
            if (_boardinEvent.NumberofPersonInQueueAtStop.Equals(0))
            {
                timer2.Stop();
                ApplicationLog.Instance.WriteInfo(string.Format("All passengers have boarded at stop# {0}.", _boardinEvent.BusStopNumber));
                timer1.Start();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            chartBusArrival.SaveImage(@"BusIntervals.png", ChartImageFormat.Png);
            chartPassangerQueue.SaveImage(@"HourlyPassngerQueue.png", ChartImageFormat.Png);
        }
    }

    public enum EventType
    {
        PersonArrival,
        BusArrial,
        PersonBoarding
    }
}
