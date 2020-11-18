using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using PBOSharp;
using PBOSharp.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PboViewer.ViewModels
{
    class PboViewerMainWindowViewModel : BaseViewModel
    {
        #region Public Members

        public string PboFileName { get; set; }
        public ObservableCollection<TreeViewItem> PboTreeViewItems { get; set; }
        public bool ProgressBarIsIndeterminate
        {
            get => _progressBarIsIndeterminate;
            set
            {
                if (_progressBarIsIndeterminate != value)
                {
                    _progressBarIsIndeterminate = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool UnpackPboButtonIsVisible
        { 
            get => _unpackPboButtonIsVisible;
            set
            {
                if (_unpackPboButtonIsVisible != value)
                {
                    _unpackPboButtonIsVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand OpenPBO { get; set; }
        public ICommand PackFolder { get; set; }
        public ICommand UnpackPbo { get; set; }

        #endregion

        #region Private Members

        private Window _mainWindow;

        private bool _progressBarIsIndeterminate;
        private bool _unpackPboButtonIsVisible;

        #endregion

        #region Constructor

        public PboViewerMainWindowViewModel()
        {
            PboTreeViewItems = new ObservableCollection<TreeViewItem>();


            OpenPBO = new RelayCommand(async () =>
            {
                CheckMainWindow();

                if (ProgressBarIsIndeterminate)
                    return;


                OpenFileDialog ofd = new OpenFileDialog
                {
                    AllowMultiple = false,
                    Title = "PboMaker | Choose a PBO file",
                    Filters = new List<FileDialogFilter>(new[]
                    {
                        new FileDialogFilter
                        {
                            Name = "PBO file",
                            Extensions = new List<string>(new [] {
                                "pbo"
                            })
                        },
                    })
                };

                string[] fileSelected = await ofd.ShowAsync(_mainWindow);
                if (fileSelected.Length == 0)
                    return;

                PboFileName = fileSelected[0];
                UnpackPboButtonIsVisible = true;


                //GenerateTreeView();
            });

            PackFolder = new RelayCommand(async () =>
            {
                CheckMainWindow();

                if (ProgressBarIsIndeterminate)
                    return;


                OpenFolderDialog ofd = new OpenFolderDialog {
                    Title = "PboMaker | Choose a folder",
                };

                // Get the selected folder
                string folderPath = await ofd.ShowAsync(_mainWindow);

                if (string.IsNullOrWhiteSpace(folderPath))
                    return;


                ProgressBarIsIndeterminate = true;

                await Task.Run(() => 
                {
                    // If the file exists, delete it
                    if (File.Exists(Path.Join(Path.GetDirectoryName(folderPath), Path.GetFileName(folderPath) + ".pbo")))
                        File.Delete(Path.Join(Path.GetDirectoryName(folderPath), Path.GetFileName(folderPath) + ".pbo"));

                    // Pack the folder as a PBO
                    PBOSharpClient pboSharpClient = new PBOSharpClient();
                    pboSharpClient.PackPBO(folderPath, Path.GetDirectoryName(folderPath), Path.GetFileName(folderPath));
                    pboSharpClient.Dispose();
                });

                ProgressBarIsIndeterminate = false;

                try
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        Process.Start("explorer.exe", Path.GetDirectoryName(folderPath));
                    else
                        Process.Start(Path.GetDirectoryName(folderPath));
                }
                catch { }
            });

            UnpackPbo = new RelayCommand(async() =>
            {
                CheckMainWindow();

                if (ProgressBarIsIndeterminate)
                    return;


                OpenFileDialog ofd = new OpenFileDialog
                {
                    AllowMultiple = false,
                    Title = "PboMaker | Choose a PBO file",
                    Filters = new List<FileDialogFilter>(new[]
                    {
                        new FileDialogFilter
                        {
                            Name = "PBO file",
                            Extensions = new List<string>(new [] {
                                "pbo"
                            })
                        },
                    })
                };

                string[] fileSelected = await ofd.ShowAsync(_mainWindow);
                if (fileSelected.Length == 0)
                    return;

                PboFileName = fileSelected[0];

                ProgressBarIsIndeterminate = true;

                await Task.Run(() => 
                {
                    // If the destination directory exists, delete it
                    if (Directory.Exists(Path.Combine(Path.GetDirectoryName(PboFileName), Path.GetFileNameWithoutExtension(PboFileName))))
                        Directory.Delete(Path.Combine(Path.GetDirectoryName(PboFileName), Path.GetFileNameWithoutExtension(PboFileName)), true);

                    // Pack the folder as a PBO
                    PBOSharpClient pboSharpClient = new PBOSharpClient();
                    pboSharpClient.ExtractAll(PboFileName, Path.Combine(Path.GetDirectoryName(PboFileName), Path.GetFileNameWithoutExtension(PboFileName)));
                    pboSharpClient.Dispose();
                });

                ProgressBarIsIndeterminate = false;

                try
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        Process.Start("explorer.exe", Path.Combine(Path.GetDirectoryName(PboFileName), Path.GetFileNameWithoutExtension(PboFileName)));
                    else
                        Process.Start(Path.Combine(Path.GetDirectoryName(PboFileName), Path.GetFileNameWithoutExtension(PboFileName)));
                }
                catch { }
            });
        }

        #endregion

        /// <summary>
        /// Instancing the main window
        /// </summary>
        private void CheckMainWindow()
        {
            if (_mainWindow != null)
                return;

            _mainWindow = ((IClassicDesktopStyleApplicationLifetime)App.Current.ApplicationLifetime).MainWindow;
        }
    }
}
