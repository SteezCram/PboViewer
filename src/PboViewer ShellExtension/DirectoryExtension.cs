using SharpShell.Attributes;
using SharpShell.SharpContextMenu;
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

namespace PboViewer_ShellExtension
{
    /// <summary>
    /// Associate a directory to the PBO Viewer application
    /// </summary>
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.Directory)]
    public class DirectoryExtension : SharpContextMenu
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
            ToolStripMenuItem mainMenu = new ToolStripMenuItem {
                Image = bitmap,
                Text = "PBO Viewer",
            };


            ToolStripMenuItem openSubMenu = new ToolStripMenuItem {
                Text = "Open with PBO Viewer",
            };
            openSubMenu.Click += (sender, args) => LaunchPboViewer();

            ToolStripMenuItem packHereSubMenu = new ToolStripMenuItem {
                Text = "Pack here",
            };
            packHereSubMenu.Click += (sender, args) => PackPboHere();

            ToolStripMenuItem packToSubMenu = new ToolStripMenuItem {
                Text = "Pack to",
            };
            packToSubMenu.Click += (sender, args) => PackPboTo();


            // Add it to the main menu
            mainMenu.DropDownItems.Add(openSubMenu);
            mainMenu.DropDownItems.Add(packHereSubMenu);
            mainMenu.DropDownItems.Add(packToSubMenu);
            // Add it to the menu
            menu.Items.Add(mainMenu);

            return menu;
        }


        /// <summary>
        /// Extract the PBO in the current folder
        /// </summary>
        private void PackPboHere()
        {
            Process.Start(new ProcessStartInfo
            {
                Arguments = $"\"{SelectedItemPaths.FirstOrDefault()}\" -ph",
                FileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "PboViewer.exe"),
                UseShellExecute = true,
                WorkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            });
        }

        /// <summary>
        /// Extract the PBO in a specific folder
        /// </summary>
        private void PackPboTo()
        {
            Process.Start(new ProcessStartInfo
            {
                Arguments = $"\"{SelectedItemPaths.FirstOrDefault()}\" -pt",
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
