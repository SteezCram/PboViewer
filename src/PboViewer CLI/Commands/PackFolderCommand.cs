using ManyConsole;
using PboViewer_Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PboViewer.Commands
{
    /// <summary>
    /// Pack a folder as a PBO
    /// </summary>
    class PackFolderCommand : ConsoleCommand
    {
        /// <summary>
        /// Directory to pack
        /// </summary>
        public string DirectoryPath { get; set; }

        /// <summary>
        /// Build a Sets application
        /// </summary>
        public PackFolderCommand()
        {
            // Register the actual command with a simple (optional) description.
            IsCommand("packFolder", "Pack a folder a PBO");

            // Required options/flags, append '=' to obtain the required value.
            HasRequiredOption("p|path=", "The full path of the directory.", p => DirectoryPath = p);
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                // If the file exists, delete it
                if (File.Exists(Path.Join(Path.GetDirectoryName(DirectoryPath), Path.GetFileName(DirectoryPath) + ".pbo")))
                    File.Delete(Path.Join(Path.GetDirectoryName(DirectoryPath), Path.GetFileName(DirectoryPath) + ".pbo"));

                // Pack the folder as a PBO
                using PBOSharpClient pboSharpClient = new PBOSharpClient(DirectoryPath);
                pboSharpClient.PackPBO(DirectoryPath, Path.GetDirectoryName(DirectoryPath), Path.GetFileName(DirectoryPath));

                Console.Out.WriteLine($"Pbo successful pack at the path: {Path.Join(Path.GetDirectoryName(DirectoryPath), Path.GetFileName(DirectoryPath) + ".pbo")}");

                return PboViewer.Success;
            }
            catch (Exception ex) {
                Console.Out.WriteLine($"Unexpected error: {Environment.NewLine}{ex}");
                return PboViewer.Failure;
            }
        }
    }
}
