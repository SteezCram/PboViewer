using ManyConsole;
using PBOSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PboViewer_CLI.Commands
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
                PBOSharpClient pboSharpClient = new PBOSharpClient();
                pboSharpClient.PackPBO(DirectoryPath, Path.GetDirectoryName(DirectoryPath), Path.GetFileName(DirectoryPath));
                pboSharpClient.Dispose();

                return PboViewer.PboViewer.Success;
            }
            catch {
                return PboViewer.PboViewer.Failure;
            }
        }
    }
}
