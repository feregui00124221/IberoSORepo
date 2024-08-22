using System;
using System.Threading;

namespace ThreadsPractice
{
    class Program
    {
        static string letters = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ";

        static char[] upperAlphabet = letters.ToCharArray();
        static char[] lowerAlphabet = letters.ToLower().ToCharArray();
        static int[] numbers = Enumerable.Range(0, 20).ToArray();

        static string upperName = "Counting uppercase letters thread";
        static string lowerName = "Counting lowercase letters thread";
        static string numbersName = "Counting numbers thread";

        static void Main(string[] args)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            Thread countThreadNumbers = new Thread(() =>
                Counting(numbers, numbers.Length, 500, source, token));
            countThreadNumbers.Name = numbersName;

            Thread countThreadLowercase = new Thread(() =>
                Counting(lowerAlphabet, lowerAlphabet.Length, 1000, source, token));
            countThreadLowercase.Name = lowerName;

            Thread countThreadUppercase = new Thread(() =>
                Counting(upperAlphabet, upperAlphabet.Length, 750, source, token));
            countThreadUppercase.Name = upperName;

            countThreadLowercase.Start();
            countThreadUppercase.Start();
            countThreadNumbers.Start();

            countThreadUppercase.Join();

            Console.WriteLine("INFO -----> All threads have finished.");
        }

        static void Counting<T>(T[] arr, int limit, int timespan, CancellationTokenSource source, CancellationToken ct)
        {
            try
            {
                for(int i = 0; i < limit; i++)
                {
                    if (Thread.CurrentThread.Name == lowerName && ct.IsCancellationRequested)
                    {
                        throw new OperationCanceledException();
                    }

                    Console.WriteLine($"{Thread.CurrentThread.Name}: {arr[i]}");
                    Thread.Sleep(timespan);
                }

                if (Thread.CurrentThread.Name == numbersName)
                    source.Cancel();

                Console.WriteLine($"{Thread.CurrentThread.Name} finished.");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name} was interrupted.");
            }
        }
    }
}