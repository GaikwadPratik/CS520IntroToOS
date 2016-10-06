using LogUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTerminal
{
    public class NextTimeIntervalGenerator
    {
        static double seed = 1000;
        static Random _random = new Random(int.Parse(seed.ToString()));

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
                _nRtnVal = _random.Next(1, 5);
            }
            catch (Exception ex)
            {
                ApplicationLog.Instance.WriteException(ex);
            }
            return _nRtnVal;
        }
    }
}
