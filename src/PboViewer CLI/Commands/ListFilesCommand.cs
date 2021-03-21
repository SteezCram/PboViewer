using ManyConsole;
using PboViewer_Lib;
using PboViewer_Lib.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PboViewer.Commands
{
    class ListFilesCommand : ConsoleCommand
    {
        /// <summary>
        /// Directory to pack
        /// </summary>
        public string PboPath { get; set; }

        /// <summary>
        /// Build a Sets application
        /// </summary>
        public ListFilesCommand()
        {
            // Register the actual command with a simple (optional) description.
            IsCommand("listFiles", "List files of a PBO");

            // Required options/flags, append '=' to obtain the required value.
            HasRequiredOption("p|path=", "The full path of the PBO.", p => PboPath = p);
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                // Pack the folder as a PBO
                using PBOSharpClient pboSharpClient = new PBOSharpClient(PboPath);
                PBO currentPbo = pboSharpClient.AnalyzePBO();

                currentPbo.Files.ForEach(x => Console.Out.WriteLine($"Path: {x.FileName}, Size: {x.OriginalSize}, Data size: {x.DataSize}, Timestamp: {x.Timestamp}, Packing method: {x.PackingMethod}"));

                return PboViewer.Success;
            }
            catch (Exception ex) {
                Console.Out.WriteLine($"Unexpected error: {Environment.NewLine}{ex}");
                return PboViewer.Failure;
            }
        }
    }
}
