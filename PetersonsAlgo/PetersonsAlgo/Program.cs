using LogUtils;
using System;
using System.Threading;

namespace PetersonsAlgo
{
    class Program
    {
        static void Main(string[] args)
        {
            PetersonTwoProcess sharedVariable = new PetersonTwoProcess();
            WorkerProcess process1 = new WorkerProcess(0, sharedVariable);
            WorkerProcess process2 = new WorkerProcess(1, sharedVariable);

            Thread t1 = new Thread(process1.RunPeterson);
            Thread t2 = new Thread(process2.RunPeterson);
            ApplicationLog.Instance.WriteInfo("Running original version of peterson's algorithm with process 1 and 2.");
            Console.WriteLine("Running original version of peterson's algorithm with process 1 and 2.");

            t1.Start();
            t2.Start();

            while (t1.IsAlive && t2.IsAlive)
            {                
                Thread.Sleep(10000);
                sharedVariable.shouldStop = true;
            }
            t1.Abort();
            t2.Abort();
            t1.Join();
            t2.Join();

            Console.WriteLine("Stopping original version of peterson's algorithm with process 1 and 2.");
            ApplicationLog.Instance.WriteInfo("Stopping original version of peterson's algorithm with process 1 and 2.");

            sharedVariable = new PetersonTwoProcess();
            WorkerProcess process3 = new WorkerProcess(0, sharedVariable);
            WorkerProcess process4 = new WorkerProcess(1, sharedVariable);

            Thread t3 = new Thread(process3.RunModifiedPeterson);
            Thread t4 = new Thread(process4.RunModifiedPeterson);

            Console.WriteLine("Running modified version of peterson's algorithm with process 3 and 4.");
            ApplicationLog.Instance.WriteInfo("Running modified version of peterson's algorithm with process 3 and 4.");

            t3.Start();
            t4.Start();

            while (t3.IsAlive && t3.IsAlive)
            {
                Thread.Sleep(10000);
                sharedVariable.shouldStop = true;
            }
            t3.Abort();
            t4.Abort();
            t3.Join();
            t4.Join();
            Console.WriteLine("Stopping modified version of peterson's algorithm with process 3 and 4.");
            ApplicationLog.Instance.WriteInfo("Stopping modified version of peterson's algorithm with process 3 and 4.");
            Console.ReadLine();
        }
    }

    class PetersonTwoProcess
    {
        public bool[] flag = new bool[2] { false, false };
        public int turn;
        public bool shouldStop = false;

        public PetersonTwoProcess()
        {
            flag[0] = false;
            flag[1] = false;
            turn = 1;
            shouldStop = false;
        }
    }

    class WorkerProcess
    {
        int id = 0;
        volatile PetersonTwoProcess sharedInstance;

        public WorkerProcess(int id, PetersonTwoProcess sharedInstance)
        {
            this.id = id;
            this.sharedInstance = sharedInstance;
        }

        public void RunPeterson()
        {
            while (true && !sharedInstance.shouldStop)
            {
                sharedInstance.flag[id] = true;
                int other = id.Equals(0) ? 1 : 0;
                sharedInstance.turn = other;

                // Case 1: Lets say P0 wants to enter but P1 doesnt: In this case,
                // sharedInstance.flag[1] == false
                // and P0 will happily enter cs

                // Case 2: If P1 wants to enter but P0 doesnt. In this case,
                // sharedInstance.flag[0] == false
                // and P1 happily

                // Case 3: If P0 makes flag[0] = true and turn = 0 and at the same
                // time P1 makes flag[1] = true and turn = 1
                // then P0 should get priority and P1 must wait. This is taken care
                // by the while condition

                // Case 4: if P1 makes flag[1] = true and turn = 1 and at the same
                // time P0 makes flag[0] = true and turn = 0
                // then P1 must get priority and P0 must wait.

                while (sharedInstance.flag[other] && sharedInstance.turn.Equals(other))
                {
                    Console.WriteLine("{0} :: Process {1} is in critical state. Process {2} is in wait state.", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"), other, id);
                    ApplicationLog.Instance.WriteInfo(string.Format("{0} :: Process {1} is in critical state. Process {2} is in wait state.", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"), other, id));
                }

                CriticalState();

                sharedInstance.flag[id] = false;
            }
        }

        public void RunModifiedPeterson()
        {
            while (true && !sharedInstance.shouldStop)
            {
                sharedInstance.flag[id] = true;
                int other = id.Equals(0) ? 1 : 0;
                sharedInstance.turn = id;

                // Case 1: Lets say P0 wants to enter but P1 doesnt: In this case,
                // sharedInstance.flag[1] == false
                // and P0 will happily enter cs

                // Case 2: If P1 wants to enter but P0 doesnt. In this case,
                // sharedInstance.flag[0] == false
                // and P1 happily

                // Case 3: If P0 makes flag[0] = true and turn = 0 and at the same
                // time P1 makes flag[1] = true and turn = 1
                // then P0 should get priority and P1 must wait. This is taken care
                // by the while condition

                // Case 4: if P1 makes flag[1] = true and turn = 1 and at the same
                // time P0 makes flag[0] = true and turn = 0
                // then P1 must get priority and P0 must wait.

                while (sharedInstance.flag[other] && sharedInstance.turn.Equals(id))
                {
                    Console.WriteLine("{0} :: Process {1} is in critical state. Process {2} is in wait state.", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"), other, id);
                    ApplicationLog.Instance.WriteInfo(string.Format("{0} :: Process {1} is in critical state. Process {2} is in wait state.", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"), other, id));
                }
                CriticalState();

                sharedInstance.flag[id] = false;
            }
        }

        private void CriticalState()
        {
            try
            {
                Console.WriteLine("Process:'{0}' entering critical state. sleeping for 1 sec", id);
                ApplicationLog.Instance.WriteInfo(string.Format("{0} :: Process {1} entering critical state. sleeping for 1 sec", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"), id));
                Thread.Sleep(10);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
