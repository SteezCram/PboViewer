using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PboViewer.OS.Windows
{
    /// <summary>
    /// Manage the Windows integration of PBO Viewer
    /// </summary>
    public static class Integration
    {
        /// <summary>
        /// Install the integration
        /// </summary>
        public static bool Install()
        {
            try
            {
                Process process = Process.Start(new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    FileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "OS", "Windows", "integration_install.bat"),
                    UseShellExecute = true,
                    Verb = "runas",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    WorkingDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "OS", "Windows"),
                });
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Uninstall the integration
        /// </summary>
        public static bool Uninstall()
        {
            try
            {
                Process process = Process.Start(new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    FileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "OS", "Windows", "integration_uninstall.bat"),
                    UseShellExecute = true,
                    Verb = "runas",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    WorkingDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "OS", "Windows"),
                });

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
