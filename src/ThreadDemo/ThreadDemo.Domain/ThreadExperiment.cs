using System.Runtime.InteropServices.Marshalling;

namespace ThreadDemo.Domain;

public class ThreadExperiments
{
    public void RunSequential()
    {
        Work("A");
        Work("B");
    }

    public void RunThreads()
    {
        Thread thread1 = new(()=> Work("A"));
        Thread thread2 = new(()=> Work("B"));
        Console.WriteLine("Starting threads........");
        thread1.Start();
        thread2.Start();
        Console.WriteLine("This line is written by main thread");
        thread1.Join();
        thread2.Join();
        Console.WriteLine("All finished");
    }

    public (int Expected, int Actual) RunCounter(bool userlock, int iterations=20_000)
    {
        int count=0;
        object gatelock=new();

        void Increment()
        {
            for (int i=0; i<= iterations; i++)
            {
                if (userlock)
                {
                    lock (gatelock)
                    {
                        count +=1;
                    }
                }
                else
                {
                    count +=1;
                }
               
            }
        }

        Thread t1 = new(Increment);
        Thread t2 = new(Increment);

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();

        return (iterations*2 , count);
    }

    public void ShowThreadProperties()
    {
        Thread thread = Thread.CurrentThread;
        Console.WriteLine($"Thread ID:{thread.ManagedThreadId}");
        Console.WriteLine($"Thread name: {thread.Name}");
        Console.WriteLine($"Thread Priority: {thread.Priority}");
        Console.WriteLine($"Thread Background: {thread.IsBackground}");
    }

    private void Work(string name)
    {
        for (int i=0 ; i<=5; i ++){
            Console.WriteLine($"{name}; Thread: {Thread.CurrentThread.ManagedThreadId}; index {i}");
            Thread.Sleep(100);
        }
    }
}
