using ManyConsole;
using PBOSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PboViewer.Commands
{
    /// <summary>
    /// Pack a folder as a PBO
    /// </summary>
    class UnpackFolderCommand : ConsoleCommand
    {
        /// <summary>
        /// Directory to pack
        /// </summary>
        public string PboPath { get; set; }

        /// <summary>
        /// Build a Sets application
        /// </summary>
        public UnpackFolderCommand()
        {
            // Register the actual command with a simple (optional) description.
            IsCommand("unpackFolder", "Unpack a PBO");

            // Required options/flags, append '=' to obtain the required value.
            HasRequiredOption("p|path=", "The full path of the PBO.", p => PboPath = p);
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                // If the destination directory exists, delete it
                if (Directory.Exists(Path.Combine(Path.GetDirectoryName(PboPath), Path.GetFileNameWithoutExtension(PboPath))))
                    Directory.Delete(Path.Combine(Path.GetDirectoryName(PboPath), Path.GetFileNameWithoutExtension(PboPath)), true);

                // Pack the folder as a PBO
                PBOSharpClient pboSharpClient = new PBOSharpClient();
                pboSharpClient.ExtractAll(PboPath, Path.Combine(Path.GetDirectoryName(PboPath), Path.GetFileNameWithoutExtension(PboPath)));
                pboSharpClient.Dispose();

                Console.Out.WriteLine($"Pbo successful unpack at the path: {Path.Combine(Path.GetDirectoryName(PboPath), Path.GetFileNameWithoutExtension(PboPath))}");

                return PboViewer.Success;
            }
            catch (Exception ex) {
                Console.Out.WriteLine($"Unexpected error: {Environment.NewLine}{ex}");
                return PboViewer.Failure;
            }
        }
    }
}
