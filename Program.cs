using System;
using System.Linq;

namespace ImageToPDF
{
    static class Program
    {
        static void Main(string[] args)
        {
            var logger = new StdoutLogger();
            var parsedArgument = new ArgumentParse(args, logger);
        }
    }
}
