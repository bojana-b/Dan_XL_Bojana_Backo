using System;
using System.Threading;

namespace Dan_XL_Bojana_Backo
{
    class Program
    {
        public static object lockObject = new object();
        Random random = new Random();
        public static string[] format = { "A3", "A4" };
        public static void PrintMessage(string message)
        {
            lock (lockObject)
            {
                Console.WriteLine(message);
            }
        }

        static void Main(string[] args)
        {
            Random random = new Random();
            int brojDokumenata = 10;

            Printer printer = new Printer(true, true);
            Thread[] dokumenta = new Thread[brojDokumenata];

            // creating threads
            for (int i = 0; i < brojDokumenata; i++)
            {
                string form = format[random.Next(format.Length)];
                if (form == "A3")
                {
                    dokumenta[i] = new Thread(new ThreadStart(printer.PrintingA3));
                }
                else
                {
                    dokumenta[i] = new Thread(new ThreadStart(printer.PrintingA4));
                }
                
                dokumenta[i].Name = "Computer" + (i + 1).ToString();
                dokumenta[i].Start();
                Thread.Sleep(100);
            }

            for (int i = 0; i < brojDokumenata; i++)
            {
                dokumenta[i].Join();
            }

            Console.WriteLine("Simulacija zavrsena");

            Console.ReadLine();
        }
    }
}
