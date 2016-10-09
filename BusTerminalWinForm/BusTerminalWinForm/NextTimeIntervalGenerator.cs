using LogUtils;
using System;

namespace BusTerminalWinForm
{
    public class NextTimeIntervalGenerator
    {
        static double seed = 1000;
        static Random _random = new Random(int.Parse(seed.ToString()));
        static BusTerminalConfiguration _internalConfiguration = null;

        public static BusTerminalConfiguration InternalConfiguration { set { _internalConfiguration = value; } }

        public static double GetTimeInterval()
        {
            double _dRtnVal = 0;
            try
            {
                _dRtnVal = -Math.Log((seed + 1) / 65536);
                seed = (25173 * seed + 13849) % 65536;
            }
            catch (Exception ex)
            {
                ApplicationLog.Instance.WriteException(ex);
            }
            return _dRtnVal;
        }

        public static int GetPersons()
        {
            int _nRtnVal = 0;
            try
            {
                if (!_internalConfiguration.SinglePersonIncrement)
                    _nRtnVal = _random.Next(1, 5);
                else
                    _nRtnVal = 1;
            }
            catch (Exception ex)
            {
                ApplicationLog.Instance.WriteException(ex);
            }
            return _nRtnVal;
        }
    }
}
