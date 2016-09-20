using LogUtils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SleepingBarbar
{
    class Program1
    {
        static void MainTas(string[] args)
        {
            try
            {
                Console.WriteLine("Opening the barber shop.");

                BarbarShop shop = new BarbarShop();

                Task barberTask = Task.Factory.StartNew(() => shop.BarberTaskAsync(), CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

                Task customerTask = Task.Factory.StartNew(() => shop.CustomerTaskAsync(), CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

                while (!barberTask.IsCompleted)
                {
                    //Console.WriteLine("Barber is still working");
                }
                //Console.WriteLine("Barber is closing shop.");

                Console.ReadLine();
            }
            catch(Exception ex)
            {
                ApplicationLog.Instance.WriteException(ex);
            }
        }
    }

    class Customer
    {
        public int Id { get; set; }
    }

    class BarbarShop
    {
        int nChairsInShop = 4;
        ConcurrentQueue<Customer> customerQueue = new ConcurrentQueue<Customer>();

        public async Task BarberTaskAsync()
        {
            bool bAreTheranyCustomersToServce = false;
            int nCustomerProcessed = 0;

            try
            {
                Console.WriteLine("Initally no customers to serve hence barber is in sleep mode");

                //In case a customer is present in queue, barber has to work.
                while (customerQueue != null && !customerQueue.IsEmpty)
                {
                    Customer nextCustomer = null;
                    //Pull out the next customer to process from the queue.
                    customerQueue.TryDequeue(out nextCustomer);

                    Console.WriteLine("Serving Customer no. {0}. Still yet {1} customers to serve. So far {2} customer served.", nextCustomer.Id, customerQueue.Count, nCustomerProcessed);
                    //Keep task in waiting to indicating the serving process of customer.
                    await Task.Delay(5000);
                    //Increasing the number of customers processed so far.
                    nCustomerProcessed++;
                    //Still customers are present to serve.
                    bAreTheranyCustomersToServce = true;
                }
                if (customerQueue.IsEmpty)
                    bAreTheranyCustomersToServce = false;
                //In case there is no customer to serve, barber can sleep.
                if (!bAreTheranyCustomersToServce)
                {
                    Console.WriteLine("No customers to serve.");
                    bAreTheranyCustomersToServce = false;
                }
            }
            catch(Exception ex)
            {
                ApplicationLog.Instance.WriteException(ex);
            }
        }

        public async Task CustomerTaskAsync()
        {
            int queueLenth = 0;

            try
            {
                //To indicate the inflow of customer delaying the task.
                await Task.Delay(new Random().Next(1, 10) * 1000);

                Console.WriteLine("A new customer has arrived.");

                if (customerQueue == null)
                    customerQueue = new ConcurrentQueue<Customer>();
                queueLenth = customerQueue.Count;
                Customer customer = new Customer() { Id = (queueLenth + 1) };

                if (!customerQueue.Contains(customer))
                {
                    if (customerQueue.Count < nChairsInShop)
                    {
                        //In case chairs in barber shops are empty then customer will enter and sit on chair to wait his turn.
                        customerQueue.Enqueue(customer);
                        Console.WriteLine("{0} number of chairs were availble so adding customer {1} to queue for servicing.", (nChairsInShop - customerQueue.Count), customer.Id);
                    }
                    else
                    {
                        //In case chairs in barber shops are full then customer will not want to wait and leaves the shop.
                        Console.WriteLine("No empty chairs are availbel. Customer {0} doesn't want to wait.");
                    }
                }
            }
            catch(Exception ex)
            {
                ApplicationLog.Instance.WriteException(ex);
            }
        }
    }
}
