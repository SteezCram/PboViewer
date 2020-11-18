using ManyConsole;
using System;
using System.Collections.Generic;

namespace PboViewer
{
    class PboViewer
    {
        /// <summary>
        /// Success code
        /// </summary>
        public const int Success = 0;
        /// <summary>
        /// Failure code
        /// </summary>
        public const int Failure = 2;

        /// <summary>
        /// Entry point of PboViewer
        /// </summary>
        static int Main(string[] args)
        {
            IEnumerable<ConsoleCommand> commands = ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(PboViewer));
            return ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
        }
    }
}
