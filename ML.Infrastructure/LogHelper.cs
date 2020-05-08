using System;
using System.IO;

namespace ML.Infrastructure
{
    public class LogHelper
    {
        private readonly FileStream ostrm;
        private readonly StreamWriter writer;
        private readonly TextWriter oldOut = Console.Out;

        public LogHelper()
        {
            try
            {
                ostrm = new FileStream("./_ConsoleWritelog.txt", FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(ostrm);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open _ConsoleWritelog.txt for writing");
                Console.WriteLine(e.Message);
                return;
            }

            Console.SetOut(writer);
        }

        public void Flush()
        {
            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();
        }

        public void DoTest()
        {
            Console.WriteLine("This is a line of text");
            Console.WriteLine("Everything written to Console.Write() or");
            Console.WriteLine("Console.WriteLine() will be written to a file");
        }
    }
}
