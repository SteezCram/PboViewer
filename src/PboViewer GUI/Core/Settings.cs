using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PboViewer.Core
{
    /// <summary>
    /// Settings of PBO Viewer
    /// </summary>
    public struct PboViewerSettings
    {
        /// <summary>
        /// If the PBO is open after an extract
        /// </summary>
        public bool OpenPackedPboInFileExplorer { get; set; }
        /// <summary>
        /// Open folder or PBO file in the context menu
        /// </summary>
        public bool OSIntegration { get; set; }
    }

    public static class Settings
    {
        public static PboViewerSettings PboViewerSettings;

        /// <summary>
        /// Load the settings
        /// </summary>
        public static void LoadSettings()
        {
            if (!File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data", "PboViewerSettings.json"))) {
                PboViewerSettings = new PboViewerSettings();
                SaveSettings();
                return;
            }

            PboViewerSettings = JsonSerializer.Deserialize<PboViewerSettings>(File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data", "PboViewerSettings.json")), new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
            });
        }

        /// <summary>
        /// Save the settings
        /// </summary>
        public static void SaveSettings()
        {
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data", "PboViewerSettings.json"), JsonSerializer.Serialize<PboViewerSettings>(PboViewerSettings, new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
            }));
        }
    }
}
