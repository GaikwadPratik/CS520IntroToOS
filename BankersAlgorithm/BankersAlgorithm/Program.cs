using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankersAlgorithm
{
    class Input
    {
        public int NumberOfResources { get; set; }
        public int NumberOfProcesses { get; set; }
        public string ClaimVector { get; set; }
        public string AllocatedResources { get; set; }
        public string MaxAllocated { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Input _in = JsonConvert.DeserializeObject<Input>(File.ReadAllText(@"json1.json"));

            int[,] _curr = new int[5, 5];
            int[,] max_claim = new int[5, 5];
            int[] avl = new int[5];
            int[] alloc = { 0, 0, 0, 0, 0 };
            int[] max_res = null;
            int[] running = null;

            int exec, _numberOfResources = _in.NumberOfResources, _numberOfProcesses = _in.NumberOfProcesses;
            int count = 0;
            bool safe = false;


            Console.Write("Enter the number of resources: {0}", _numberOfResources);
            //int.TryParse(Console.ReadLine(), out _numberOfResources);
            max_res = new int[_numberOfResources];
            Console.WriteLine();

            Console.Write("Enter the number of processes: {0}", _numberOfProcesses);
            //int.TryParse(Console.ReadLine(), out _numberOfProcesses);
            running = new int[_numberOfProcesses];
            Console.WriteLine();

            for (int _index = 0; _index < _numberOfProcesses; _index++)
            {
                running[_index] = 1;
                count++;
            }


            //Console.WriteLine("Enter Claim Vector: ");
            string[] _strTempArr = _in.ClaimVector.Split(' ');
            for (int _index = 0; _index < _numberOfResources; _index++)
            {
                int _nInput = 0;
                int.TryParse(_strTempArr[_index], out _nInput);
                max_res[_index] = _nInput;
            }

            //Console.WriteLine("Enter Allocated Resource Table: ");
            int _counter = 0;
            _strTempArr = _in.AllocatedResources.Split(' ');
            for (int _index = 0; _index < _numberOfProcesses; _index++)
            {
                for (int _innerIndex = 0; _innerIndex < _numberOfResources; _innerIndex++)
                {
                    int _nInput = 0;
                    int.TryParse(_strTempArr[_counter++], out _nInput);
                    _curr[_index, _innerIndex] = _nInput;
                }
            }

            //Console.WriteLine("Enter Maximum Claim table: ");
            _strTempArr = _in.MaxAllocated.Split(' ');
           _counter = 0;
            for (int _index = 0; _index < _numberOfProcesses; _index++)
            {
                for (int _innerIndex = 0; _innerIndex < _numberOfResources; _innerIndex++)
                {
                    int _nInput = 0;
                    int.TryParse(_strTempArr[_counter++], out _nInput);
                    max_claim[_index, _innerIndex] = _nInput;
                }
            }


            Console.WriteLine("The Claim Vector is: ");
            for (int _index = 0; _index < _numberOfResources; _index++)
                Console.Write("{0} ", max_res[_index]);


            Console.WriteLine("The Allocated Resource Table:");
            for (int _index = 0; _index < _numberOfProcesses; _index++)
            {
                for (int _innerIndex = 0; _innerIndex < _numberOfResources; _innerIndex++)
                    Console.Write(" {0}", _curr[_index, _innerIndex]);
                Console.WriteLine();
            }

            Console.WriteLine("The Maximum Claim Table: ");
            for (int _index = 0; _index < _numberOfProcesses; _index++)
            {
                for (int _innerIndex = 0; _innerIndex < _numberOfResources; _innerIndex++)
                    Console.Write(" {0}", max_claim[_index, _innerIndex]);
                Console.WriteLine();
            }

            for (int _index = 0; _index < _numberOfProcesses; _index++)
                for (int _innerIndex = 0; _innerIndex < _numberOfResources; _innerIndex++)
                    alloc[_innerIndex] += _curr[_index, _innerIndex];

            Console.WriteLine("Allocated resources: ");
            for (int _index = 0; _index < _numberOfResources; _index++)
                Console.Write("{0} ", alloc[_index]);
            for (int _index = 0; _index < _numberOfResources; _index++)
                avl[_index] = max_res[_index] - alloc[_index];

            Console.WriteLine();
            Console.WriteLine("Available resources: ");
            for (int _index = 0; _index < _numberOfResources; _index++)
                Console.Write("{0} ", avl[_index]);
            Console.WriteLine();

            while (count != 0)
            {
                safe = false;
                for (int _index = 0; _index < _numberOfProcesses; _index++)
                {
                    if (running[_index].Equals(1))
                    {
                        exec = 1;
                        for (int _innerIndex = 0; _innerIndex < _numberOfResources; _innerIndex++)
                        {
                            if (max_claim[_index, _innerIndex] - _curr[_index, _innerIndex] > avl[_innerIndex])
                            {
                                exec = 0;
                                break;
                            }
                        }

                        if (exec.Equals(0))
                        {
                            Console.WriteLine("Process# {0} is executing.", _index + 1);
                            running[_index] = 0;
                            count--;
                            safe = true;
                            for (int _innerIndex = 0; _innerIndex < _numberOfResources; _innerIndex++)
                                avl[_innerIndex] += _curr[_index, _innerIndex];
                            break;
                        }
                    }
                }

                if (!safe)
                {
                    Console.WriteLine("The processes are in unsafe state.");
                    break;
                }

                if (safe)
                    Console.WriteLine("The process is in safe state.");


                Console.Write("Available vector: ");
                for (int _index = 0; _index < _numberOfResources; _index++)
                    Console.Write("{0} ", avl[_index]);
            }
            Console.ReadKey();
        }
    }
}
