using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PboViewer.Core
{
    public class IO
    {
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
    }
}
