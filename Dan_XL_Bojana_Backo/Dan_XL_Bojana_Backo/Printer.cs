using System;
using System.Threading;

namespace Dan_XL_Bojana_Backo
{
    enum PrinterAction : int
    {
        WAIT=0,
        PRINTING
    }
    class Printer
    {
        string[] orientation = { "portrait", "landscape" };
        string[] color = { "DarkYellow", "DarkBlue", "DarkGreen", "DarkRed", "DarkMagenta", "Blue", "Green", "Magenta" };
        private bool printerA3IsFree;
        private bool printerA4IsFree;
        public object checkLock;

        // Objects for communication between threads
        private AutoResetEvent FreeA3;
        private AutoResetEvent FreeA4;

        Random random = new Random();

        public static int duzinaReda1 = 0;
        public static int duzinaReda2 = 0;

        public Printer(bool printerA3IsFree, bool printerA4IsFree)
        {
            this.printerA3IsFree = printerA3IsFree;
            this.printerA4IsFree = printerA4IsFree;
            checkLock = new object();
            FreeA3 = new AutoResetEvent(false);
            FreeA4 = new AutoResetEvent(false);
        }

        // Function for printing A3 documents in printer 1
        public void PrintingA3()
        {
            // local variables, thread safe
            string documentToPrint = Thread.CurrentThread.Name;
            PrinterAction action = PrinterAction.WAIT;

            string c = color[random.Next(color.Length)];
            ConsoleColor consoleColor = ConsoleColor.White;
            try
            {
                consoleColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), c, true);
            }
            catch (Exception)
            {

            }
            Console.ForegroundColor = consoleColor;
            Program.PrintMessage(string.Format("{0} is sent a print request for printing A3 paper format. " +
                "\nColor: [{1}]. Orientation: [{2}].", documentToPrint, c, orientation[random.Next(orientation.Length)]));
            Console.ResetColor();

            // lock critical section of code
            lock (checkLock)
            {
                if (printerA3IsFree)
                {
                    printerA3IsFree = false;
                    action = PrinterAction.PRINTING;
                }
                else
                {
                    duzinaReda1++;
                    action = PrinterAction.WAIT;
                }
            }
            switch (action)
            {
                case PrinterAction.WAIT:
                    {
                        Program.PrintMessage(string.Format("{0} waiting for the printer A3 to release.", documentToPrint));
                        FreeA3.WaitOne();
                        lock (checkLock)
                        {
                            duzinaReda1--;
                        }
                    }
                    break;
                case PrinterAction.PRINTING:
                    {
                        
                    }
                    break;
            }
            Program.PrintMessage(string.Format("{0} starts printing.", documentToPrint));
            FreePrinterA3();
        }
        public void FreePrinterA3()
        {
            Thread.Sleep(1000);

            lock (checkLock)
            {
                if (duzinaReda1 > 0)
                {
                    FreeA3.Set();
                }
                else
                {
                    printerA3IsFree = true;
                }
                
                Program.PrintMessage(string.Format("{0} ends printing. It's free to print another A4 document.", Thread.CurrentThread.Name));
            }
        }

        // Function for printing A4 documents in printer 2
        public void PrintingA4()
        {
            string documentToPrint = Thread.CurrentThread.Name;
            PrinterAction action = PrinterAction.WAIT;

            string c = color[random.Next(color.Length)];
            ConsoleColor consoleColor = ConsoleColor.White;
            try
            {
                consoleColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), c, true);
            }
            catch (Exception)
            {

            }
            Console.ForegroundColor = consoleColor;
            Program.PrintMessage(string.Format("{0} is sent a print request for printing A4 paper format. " +
                "\nColor: [{1}]. Orientation: [{2}].", documentToPrint, c, orientation[random.Next(orientation.Length)]));
            Console.ResetColor();
            lock (checkLock)
            {

                if (printerA4IsFree)
                {
                    printerA4IsFree = false;
                    action = PrinterAction.PRINTING;
                }
                else
                {
                    duzinaReda2++;
                    action = PrinterAction.WAIT;
                }

            }
            switch (action)
            {
                case PrinterAction.WAIT:
                    {
                        Program.PrintMessage(string.Format("{0} waiting for the printer A4 to release.", documentToPrint));
                        FreeA3.WaitOne();
                        lock (checkLock)
                        {
                            duzinaReda2--;
                        }
                    }
                    break;
                case PrinterAction.PRINTING:
                    {
                        
                    }
                    break;
            }
            Program.PrintMessage(string.Format("{0} starts printing.", documentToPrint));
            FreePrinterA4();
        }


        public void FreePrinterA4()
        {
            Thread.Sleep(1000);

            lock (checkLock)
            {
                if (duzinaReda2 > 0)
                {
                    FreeA4.Set();
                }
                else
                {
                    printerA4IsFree = true;
                }
                
                Program.PrintMessage(string.Format("{0} ends printing. It's free to print another A4 document.", Thread.CurrentThread.Name));
            }
        }
    }
}
