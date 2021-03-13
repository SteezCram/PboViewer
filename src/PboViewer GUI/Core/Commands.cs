using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using PBOSharp;
using PboViewer.ViewModels;
using PboViewer.Views;
using PboViewer.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PboViewer.Core
{
    public class Commands
    {
        /// <summary>
        /// Add a file to the PBO
        /// </summary>
        /// 
        /// <param name="currentPath">Path to add the files</param>
        public static async Task AddFile(string currentPath)
        {
            Window mainWindow = ((IClassicDesktopStyleApplicationLifetime)App.Current.ApplicationLifetime).MainWindow;

            OpenFileDialog ofd = new OpenFileDialog {
                AllowMultiple = true,
                Title = "PBO Viewer | Choose a PBO file",
            };

            string[] fileSelected = await ofd.ShowAsync(((IClassicDesktopStyleApplicationLifetime)App.Current.ApplicationLifetime).MainWindow);
            if (fileSelected.Length == 0)
                return;

            await Task.Run(() => fileSelected.ToList().ForEach(x => File.Copy(x, Path.Combine(currentPath, Path.GetFileName(x)), true)));
        }

        /// <summary>
        /// Add a file to the PBO
        /// </summary>
        /// 
        /// <param name="currentPath">Path to add the files</param>
        public static async Task AddFolder(string currentPath)
        {
            Window mainWindow = ((IClassicDesktopStyleApplicationLifetime)App.Current.ApplicationLifetime).MainWindow;

            OpenFolderDialog ofd = new OpenFolderDialog {
                Title = "PBO Viewer | Choose a folder",
            };

            // Get the selected folder
            string folderPath = await ofd.ShowAsync(((IClassicDesktopStyleApplicationLifetime)App.Current.ApplicationLifetime).MainWindow);

            if (string.IsNullOrWhiteSpace(folderPath))
                return;

            await Task.Run(() => IO.DirectoryCopy(folderPath, Path.Combine(currentPath, Path.GetFileName(folderPath))));
        }

        /// <summary>
        /// Make the checksum of the PBO
        /// </summary>
        /// 
        /// <param name="editorPath">PBO path to make the checksum</param>
        /// 
        /// <returns></returns>
        public static async Task Checksum(string filePath)
        {
            ChecksumWindow checksumWindow = new ChecksumWindow();
            //string filePath = !isFile ? Path.Combine(Path.GetDirectoryName(editorPath), Path.GetFileName(editorPath) + ".pbo") : editorPath;

            if (!File.Exists(filePath))
            {
                checksumWindow.Init(filePath, "File not found");
                return;
            }

            using SHA256 sha256 = SHA256.Create();
            using Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            byte[] hash = await Task.Run(() => sha256.ComputeHash(stream));
            string shaChecksum = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();

            checksumWindow.Init(filePath, shaChecksum);
        }

        /// <summary>
        /// Copy the item path to a new destination
        /// </summary>
        /// 
        /// <param name="itemPath">Item path to copy</param>
        /// 
        /// <returns></returns>
        public static async Task CopyTo(string itemPath)
        {
            Window mainWindow = ((IClassicDesktopStyleApplicationLifetime)App.Current.ApplicationLifetime).MainWindow;

            if ((File.GetAttributes(itemPath) & FileAttributes.Directory) == FileAttributes.Directory)
            {
                OpenFolderDialog ofd = new OpenFolderDialog {
                    Title = $"PBO Viewer | Where to save {Path.GetFileName(itemPath)}",
                };

                // Get the selected folder
                string folderPath = await ofd.ShowAsync(mainWindow);

                if (string.IsNullOrWhiteSpace(folderPath))
                    return;

                string destinationPath = Path.Combine(folderPath, Path.GetFileName(itemPath));

                if (Directory.Exists(destinationPath))
                    Directory.Delete(destinationPath);

                IO.DirectoryCopy(itemPath, destinationPath);
                return;
            }


            SaveFileDialog sfd = new SaveFileDialog {
                DefaultExtension = "pbo",
                Title = $"PBO Viewer | Where to save {Path.GetFileName(itemPath)}",
                InitialFileName = Path.GetFileName(itemPath),
            };

            // Get the selected folder
            string filePath = await sfd.ShowAsync(mainWindow);

            if (string.IsNullOrWhiteSpace(filePath))
                return;

            if (File.Exists(filePath))
                File.Delete(filePath);

            File.Copy(itemPath, filePath);
        }

        /// <summary>
        /// Delete the item path
        /// </summary>
        /// 
        /// <param name="itemPath">Item path to delete</param>
        /// 
        /// <returns></returns>
        public static async Task Delete(string itemPath)
        {
            if ((File.GetAttributes(itemPath) & FileAttributes.Directory) == FileAttributes.Directory) {
                await Task.Run(() => Directory.Delete(itemPath, true));
                return;
            }

            File.Delete(itemPath);
        }

        /// <summary>
        /// Open a folder into PBO Viewer
        /// </summary>
        /// <param name="window"></param>
        public static async void OpenFolder(string directoryPath = null)
        {
            Window mainWindow = ((IClassicDesktopStyleApplicationLifetime)App.Current.ApplicationLifetime).MainWindow;

            OpenFolderDialog ofd = new OpenFolderDialog {
                Title = "PBO Viewer | Choose a folder",
            };

            // Get the selected folder
            string folderPath = directoryPath is null ? await ofd.ShowAsync(mainWindow) : directoryPath;

            if (string.IsNullOrWhiteSpace(folderPath))
                return;

            // Init the editor
            EditorView editorView = new EditorView();
            editorView.Init(folderPath);
            ((PboViewerMainWindowViewModel)((PboViewerMainWindow)mainWindow).DataContext).Child = editorView;
            // Set the new title
            mainWindow.Title = $"PBO Viewer | {folderPath}";
            ((PboViewerMainWindow)mainWindow).Title.Text = $"PBO Viewer | {folderPath}";
        }

        /// <summary>
        /// Open a PBO into PBO Viewer
        /// </summary>
        /// <param name="window"></param>
        public static async void OpenPBO(string filePath = null)
        {
            Window mainWindow = ((IClassicDesktopStyleApplicationLifetime)App.Current.ApplicationLifetime).MainWindow;

            OpenFileDialog ofd = new OpenFileDialog
            {
                AllowMultiple = false,
                Title = "PBO Viewer | Choose a PBO file",
                Filters = new List<FileDialogFilter>(new[]
                {
                    new FileDialogFilter
                    {
                        Name = "PBO file",
                        Extensions = new List<string>(new[] {
                            "pbo"
                        })
                    },
                })
            };

            string[] fileSelected = filePath is null ? await ofd.ShowAsync(mainWindow) : new[] { filePath };
            if (fileSelected.Length == 0)
                return;

            // Init the editor
            EditorView editorView = new EditorView();
            editorView.Init(fileSelected[0]);
            ((PboViewerMainWindowViewModel)((PboViewerMainWindow)mainWindow).DataContext).Child = editorView;
            // Set the new title
            mainWindow.Title = $"PBO Viewer | {fileSelected[0]}";
            ((PboViewerMainWindow)mainWindow).Title.Text = $"PBO Viewer | {fileSelected[0]}";
        }

        /// <summary>
        /// Pack the PBO
        /// </summary>
        /// 
        /// <param name="editorPath">Editor path to pack the entire PBO</param>
        /// 
        /// <returns></returns>
        public static async Task<string> PackPBO(string editorPath, string pboPath = null)
        {
            string filePath = pboPath;
            bool openFileExplorer = false;

            if (pboPath is null)
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    DefaultExtension = "pbo",
                    Title = $"PBO Viewer | Where to save {Path.GetFileName(editorPath)}.pbo",
                    InitialFileName = Path.GetFileName(editorPath),
                };

                // Get the selected folder
                filePath = await sfd.ShowAsync(((IClassicDesktopStyleApplicationLifetime)App.Current.ApplicationLifetime).MainWindow);

                if (string.IsNullOrWhiteSpace(filePath))
                    return null;

                openFileExplorer = true;
            }

            await Task.Run(() =>
            {
                // If the file exists, delete it
                if (File.Exists(filePath))
                    File.Delete(filePath);

                // Pack the folder as a PBO
                using PBOSharpClient pboSharpClient = new PBOSharpClient();
                pboSharpClient.PackPBO(editorPath, Path.GetDirectoryName(filePath), Path.GetFileName(filePath));
            });


            if (!openFileExplorer)
                return filePath;

            try
            {
                if (Settings.PboViewerSettings.OpenPackedPboInFileExplorer) {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        Process.Start("explorer.exe", Path.GetDirectoryName(editorPath));
                    else
                        Process.Start(new ProcessStartInfo {
                            FileName = Path.GetDirectoryName(editorPath),
                            UseShellExecute = true,
                        });
                }
            }
            catch { }

            return filePath;
        }

        /// <summary>
        /// Unpack a PBO to a folder
        /// </summary>
        /// 
        /// <param name="pboFile">PBO file to unpack</param>
        /// <param name="destinationFolder">Destination folder to unpack the content of the PBO</param>
        /// 
        /// <returns></returns>
        public static async Task UnpackPBO(string pboFile, string destinationFolder = null)
        {
            Window mainWindow = ((IClassicDesktopStyleApplicationLifetime)App.Current.ApplicationLifetime).MainWindow;

            OpenFolderDialog ofd = new OpenFolderDialog {
                Title = "PBO Viewer | Choose a folder",
            };

            destinationFolder = destinationFolder is not null ? destinationFolder : await ofd.ShowAsync(mainWindow);
            if (string.IsNullOrWhiteSpace(destinationFolder))
                return;


            await Task.Run(() =>
            {
                // If the file exists, delete it
                if (Directory.Exists(Path.Combine(destinationFolder, Path.GetFileNameWithoutExtension(pboFile))))
                    Directory.Delete(Path.Combine(destinationFolder, Path.GetFileNameWithoutExtension(pboFile)), true);

                // Pack the folder as a PBO
                using PBOSharpClient pboSharpClient = new PBOSharpClient();
                pboSharpClient.ExtractAll(pboFile, Path.Combine(destinationFolder, Path.GetFileNameWithoutExtension(pboFile)));
            });


            try
            {
                if (Settings.PboViewerSettings.OpenPackedPboInFileExplorer)
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        Process.Start("explorer.exe", Path.Combine(destinationFolder, Path.GetFileNameWithoutExtension(pboFile)));
                    else
                        Process.Start(new ProcessStartInfo {
                            FileName = Path.Combine(destinationFolder, Path.GetFileNameWithoutExtension(pboFile)),
                            UseShellExecute = true,
                        });
                }
            }
            catch { }
        }

        /// <summary>
        /// Start a file
        /// </summary>
        /// 
        /// <param name="filePath">File path to start</param>
        public static void StartFile(string filePath)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true,
            });
        }
    }
}
