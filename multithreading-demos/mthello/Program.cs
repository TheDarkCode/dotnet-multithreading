using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace mthello
{

    delegate void BinaryOperation(int x, int y);

    class Program
    {
        /*

            ::: General Notes on Multi Threading :::

            Threads are core concurrent programming construct in .NET

            Use: System.Threading.Thread (Old), System.Threading (Current)

                -   Contained in/within single process.
                -   Share access to any/all data within process/AppDomain
                -   Independently prioritized & scheduled for CPU time by OS
                -   Leveraging Threads (Manual/Explicit; Thread Pool; Async I/O)

            QueueUserWorkItem / Delegates ::
                Dispatch a pooled thread to call a method that may take a long time to complete.

            Delegates: provide type-aware interface to thread pool
                - Classes that represent a method call (signature and target instance)
                - Compiled-generated BeginInvoke matches method signature
                - CLR implements BeginInvoke at run-time to queue thread pool request


            Async I/O is inverse use model. Enqueuing request to perform I/O not dispatch thread.
                - Only when I/O completes do you dispatch thread.

        */

        //static volatile bool cancel = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Multi Threading Demos\r\n");
            Console.WriteLine("[{0}], Main called", Thread.CurrentThread.ManagedThreadId);

            Console.WriteLine("[{0}] Processor/core count = {1} \r\n[{0}] Is 64 Bit OS = {2}",
                                Thread.CurrentThread.ManagedThreadId,
                                Environment.ProcessorCount,
                                Environment.Is64BitOperatingSystem);

            // ThreadPool.QueueUserWorkItem(DisplayMessage, "Hello, World!");

            //Thread t = new Thread(SayHello);
            //t.Name = "Hello Thread";
            //t.Priority = ThreadPriority.BelowNormal;
            //t.IsBackground = true;
            //t.Start(10);

            //for (int n = 0; n < 10; n++)
            //{
            //    ThreadPool.QueueUserWorkItem(SayHello, n);
            //}
#if (false)
            Add(2, 2);
#else
            BinaryOperation asyncAdd = Add;
            asyncAdd.BeginInvoke(2, 2, null, null);
            Thread.Sleep(1000);
#endif
            Thread.Sleep(rng.Next(1000, 5000));
            Console.WriteLine("[{0}] Main done", Thread.CurrentThread.ManagedThreadId);

            //Thread.Sleep(5000);
            // Keep the console open in debug mode.
            Console.WriteLine(System.Environment.NewLine);
            Console.WriteLine("Press any key to exit . . . ");
            Console.ReadKey();

            //cancel = true;
            //t.Join();

            //Console.WriteLine("Done");
        }

        static Random rng = new Random();

        static void SayHello(object arg)
        {
            //while (!cancel)
            //{
            //    Console.WriteLine("[{0}] Hello, world!", Thread.CurrentThread.ManagedThreadId);
            //    Thread.Sleep(1000);
            //}

            Thread.Sleep(rng.Next(250, 500));
            int n = (int)arg;

            Console.WriteLine("[{0}] Hello, World {1}! ({2})",
                Thread.CurrentThread.ManagedThreadId,
                n,
                Thread.CurrentThread.IsBackground);
        }

        static void DisplayMessage(object stateArg)
        {
            Console.WriteLine(stateArg);
        }

        static void Add(int a, int b)
        {
            Console.WriteLine("[{0}] Add({1}, {2}) => {3}",
                Thread.CurrentThread.ManagedThreadId,
                a, b, (a + b));
        }
    }
}
