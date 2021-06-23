using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PboViewer.Core
{
    /// <summary>
    /// Helper IO class
    /// </summary>
    public class IO
    {
        public static FileSystemWatcher EditorWatcher { get; set; }
        public static Timer EditorTimer { get; set; }


        /// <summary>
        /// Copy a directory
        /// </summary>
        /// 
        /// <param name="sourcePath">Source path of the directory</param>
        /// <param name="destinationPath">Destination path of the directory</param>
        public static void DirectoryCopy(string sourcePath, string destinationPath)
        {
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));

            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), true);
        }


        /// <summary>
        /// Watch the editor path to report changes
        /// </summary>
        public static void Watch(string editorPath, string pboPath)
        {
            int eventsTriggered = 0;

            EditorWatcher = new FileSystemWatcher(editorPath) { 
                IncludeSubdirectories = true,
                EnableRaisingEvents = true,
            };

            EditorWatcher.Changed += (sender, e) => {
                eventsTriggered++;
            };
            EditorWatcher.Created += (sender, e) => {
                eventsTriggered++;
            };
            EditorWatcher.Deleted += (sender, e) => {
                eventsTriggered++;
            };
            EditorWatcher.Renamed += (sender, e) => {
                eventsTriggered++;
            };

            EditorTimer = new Timer(100);
            EditorTimer.Elapsed += (sender, e) =>
            {
                lock ((object)eventsTriggered) {
                    if (eventsTriggered > 0) {
                        Commands.PackPBO(editorPath, pboPath).Wait();
                        eventsTriggered = 0;
                    }
                }
            };
            EditorTimer.Enabled = true;
        }

        /// <summary>
        /// Unwatch the editor path
        /// </summary>
        public static void Unwatch()
        {
            if (EditorWatcher != null)
            {
                EditorWatcher.Dispose();
                EditorWatcher = null;
            }

            if (EditorTimer != null)
            {
                EditorTimer.Dispose();
                EditorTimer = null;
            }
        }
    }
}
