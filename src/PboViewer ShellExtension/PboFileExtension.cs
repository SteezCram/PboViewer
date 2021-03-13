using PboViewer_ShellExtension;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;
using SharpShell.SharpIconHandler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PboViewer_PBO_ShellExtension
{
    /// <summary>
    /// Associate a PBO file to the PBO Viewer application
    /// </summary>
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".pbo")]
    public class PboFileExtension : SharpContextMenu
    {
        /// <summary>
        /// Always show the menu
        /// </summary>
        /// 
        /// <returns></returns>
        protected override bool CanShowMenu()
        {
            return true;
        }

        /// <summary>
        /// Create the context menu strip
        /// </summary>
        /// 
        /// <returns></returns>
        protected override ContextMenuStrip CreateMenu()
        {
            Bitmap bitmap = new Bitmap(16, 16);
            Graphics graphic = Graphics.FromImage(bitmap);
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphic.DrawImage(Resources.PBOViewer_Icon, 0, 0, 16, 16);


            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem mainMenu = new ToolStripMenuItem
            {
                Image = bitmap,
                Text = "PBO Viewer",
            };


            ToolStripMenuItem openSubMenu = new ToolStripMenuItem {
                Text = "Open with PBO Viewer",
            };
            openSubMenu.Click += (sender, args) => LaunchPboViewer();

            ToolStripMenuItem extractHereSubMenu = new ToolStripMenuItem {
                Text = "Extract here",
            };
            extractHereSubMenu.Click += (sender, args) => ExtractPboHere();

            ToolStripMenuItem extractToSubMenu = new ToolStripMenuItem {
                Text = "Extract to",
            };
            extractToSubMenu.Click += (sender, args) => ExtractPboTo();


            // Add it to the main menu
            mainMenu.DropDownItems.Add(openSubMenu);
            mainMenu.DropDownItems.Add(extractHereSubMenu);
            mainMenu.DropDownItems.Add(extractToSubMenu);
            // Add it to the menu
            menu.Items.Add(mainMenu);

            return menu;
        }


        /// <summary>
        /// Extract the PBO in the current folder
        /// </summary>
        private void ExtractPboHere()
        {
            Process.Start(new ProcessStartInfo
            {
                Arguments = $"\"{SelectedItemPaths.FirstOrDefault()}\" -eh",
                FileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "PboViewer.exe"),
                UseShellExecute = true,
                WorkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            });
        }

        /// <summary>
        /// Extract the PBO in a specific folder
        /// </summary>
        private void ExtractPboTo()
        {
            Process.Start(new ProcessStartInfo
            {
                Arguments = $"\"{SelectedItemPaths.FirstOrDefault()}\" -et",
                FileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "PboViewer.exe"),
                UseShellExecute = true,
                WorkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            });
        }

        /// <summary>
        /// Start PBO Viewer with the selected item path
        /// </summary>
        private void LaunchPboViewer()
        {
            Process.Start(new ProcessStartInfo
            {
                Arguments = $"\"{SelectedItemPaths.FirstOrDefault()}\"",
                FileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "PboViewer.exe"),
                UseShellExecute = true,
                WorkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            });
        }
    }
}
