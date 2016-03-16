using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace mtadd
{
    class Program
    {
        /*

            Works on Multi Core Processors should return value Sum = 10

            If on Single Core Processor should return Sum equals between 1-10
                - This is due to thread suspensions. See #region sum++


            Intel example x86 (non-atomic) operation:

                mov eax, dword ptr [sum]
                inc eax
                mov dword ptr [sum], eax


            Solution #1: Atomic Updates
                - Atomic updates of word-sized data:

                    mov ecx, dword ptr [sum]
                    mov eax, 1
                    lock xadd dword ptr [ecx], eax

                - "lock" is an x86 instruction prefix that coordinates 
                   multi(core/cpu) access to memory

                - In .NET:
                    processor-independent suite :: System.Threading.Interlocked.Xxx

        */

        static int sum = 0;
        
        static void Main()
        {
            Thread[] threads = new Thread[10];

            for (int n = 0; n < threads.Length; n++)
            {
                threads[n] = new Thread(AddOne);
                threads[n].Start();
            }

            for (int n = 0; n < threads.Length; n++)
            {
                threads[n].Join();
            }

            Console.WriteLine(
                "[{0}] sum = {1}", Thread.CurrentThread.ManagedThreadId, sum);

            //Thread.Sleep(5000);
            // Keep the console open in debug mode.
            Console.WriteLine(System.Environment.NewLine);
            Console.WriteLine("Press any key to exit . . . ");
            Console.ReadKey();
        }

        static void AddOne ()
        {
            Console.WriteLine("[{0}] AddOne called", Thread.CurrentThread.ManagedThreadId);

            //Only on Multi-core
            //sum++;

            //Sample to simulate issue
            //#region sum++
            //    int temp = sum;
            //    temp++;
            //    Thread.Sleep(1);
            //    // Causes program to reset sum back to stored temporary after yielding CPU to other threads
            //    sum = temp;
            //#endregion

            //Atomic Update (Works on Single Core)
            Interlocked.Increment(ref sum);
        }
    }
}
