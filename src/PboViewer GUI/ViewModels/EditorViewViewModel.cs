using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using PboViewer.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Path = System.IO.Path;

namespace PboViewer.ViewModels
{
    public class EditorViewViewModel : BaseViewModel
    {
        public ObservableCollection<ListBoxItem> FolderItems
        {
            get => _folderItems;
            set
            {
                if (value != _folderItems)
                {
                    _folderItems = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool HasPBO
        {
            get => _hasPBO;
            set
            {
                if (value != _hasPBO)
                {
                    _hasPBO = value;
                    OnPropertyChanged();
                }
            }
        }


        public ICommand AddFile { get; set; }
        public ICommand AddFolder { get; set; }
        public ICommand Checksum { get; set; }
        public ICommand PackPBO { get; set; }
        public ICommand UnpackPBO { get; set; }


        private string _currentPath;
        private string _editorPath;
        private ObservableCollection<ListBoxItem> _folderItems;
        private bool _hasPBO = false;
        private string _pboPath;


        public EditorViewViewModel(string editorPath, string pboPath = null)
        {
            _editorPath = editorPath;
            _pboPath = pboPath;
            if (pboPath is not null)
                HasPBO = true;


            AddFile = new RelayCommand(async() => { 
                await Commands.AddFile(_currentPath); 
                DrawFolderItems(_currentPath); 

                if (HasPBO)
                    _pboPath = await Commands.PackPBO(_editorPath, _pboPath);
            });

            AddFolder = new RelayCommand(async() => { 
                await Commands.AddFolder(_currentPath); 
                DrawFolderItems(_currentPath); 

                if (HasPBO)
                    _pboPath = await Commands.PackPBO(_editorPath, _pboPath);
            });

            Checksum = new RelayCommand(async () => {
                await Commands.Checksum(_pboPath); 
            });

            PackPBO = new RelayCommand(async() => {
                _pboPath = await Commands.PackPBO(_editorPath, _pboPath); 
                HasPBO = true; 
            });

            UnpackPBO = new RelayCommand(async() => {
                await Commands.UnpackPBO(_pboPath);
            });
        }


        public void DrawFolderItems(string directoryPath)
        {
            _currentPath = directoryPath;
            FolderItems = new ObservableCollection<ListBoxItem>();

            GeometryDrawing folderDrawing = ((Style)App.Current.Styles.ToArray()[3].Children[0].Children[0]).Resources["Material.Folder"] as GeometryDrawing;
            folderDrawing.Brush = Brushes.Wheat;
            GeometryDrawing extractDrawing = ((Style)App.Current.Styles.ToArray()[3].Children[0].Children[0]).Resources["Material.PackageDown"] as GeometryDrawing;
            extractDrawing.Brush = Brushes.LimeGreen;
            GeometryDrawing deleteDrawing = ((Style)App.Current.Styles.ToArray()[3].Children[0].Children[0]).Resources["Material.Delete"] as GeometryDrawing;
            deleteDrawing.Brush = Brushes.Red;
            GeometryDrawing renameDrawing = ((Style)App.Current.Styles.ToArray()[3].Children[0].Children[0]).Resources["Material.RenameBox"] as GeometryDrawing;
            renameDrawing.Brush = Brushes.MediumPurple;


            // Create the folder list box item
            if (directoryPath != _editorPath)
            {
                ListBoxItem previousFolderListBoxItem = new ListBoxItem
                {
                    Tag = Path.GetDirectoryName(directoryPath),
                    Content = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Children =
                        {
                            new DrawingPresenter
                            {
                                Height = 18,
                                Width = 18,
                                Drawing = folderDrawing,
                            },
                            new TextBlock
                            {
                                FontSize = 14,
                                Foreground = Brushes.Wheat,
                                Text = "..",
                                Margin = new Thickness(5, 0, 0, 0),
                            }
                        },
                    },
                };
                // Navigate throught the root directory
                previousFolderListBoxItem.DoubleTapped += (sender, e) =>
                {
                    string listBoxItemDirectoryPath = (string)((ListBoxItem)sender).Tag;

                    // If the directory path is a drive name
                    if (listBoxItemDirectoryPath is not null)
                        // Display the folder item
                        DrawFolderItems(listBoxItemDirectoryPath);
                };
                //previousFolderListBoxItem.Tapped += (sender, e) => { FolderItemDetail.Child = null; };
                
                FolderItems.Add(previousFolderListBoxItem);
            }


            // Display all the folder in the current directory
            Directory.GetDirectories(directoryPath).ToList().ForEach(folder =>
            {
                // Create the folder list box item
                ListBoxItem folderListBoxItem = new ListBoxItem
                {
                    Tag = folder,
                    Content = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Children =
                        {
                            new DrawingPresenter
                            {
                                Height = 18,
                                Width = 18,
                                Drawing = folderDrawing,
                            },
                            new TextBlock
                            {
                                FontSize = 14,
                                Foreground = Brushes.Wheat,
                                Text = Path.GetFileName(folder),
                                Margin = new Thickness(5, 0, 0, 0),
                            }
                        },
                    },
                };
                // Navigate through directory
                folderListBoxItem.DoubleTapped += (sender, e) => {
                    string folderPath = ((ListBoxItem)sender).Tag.ToString();
                    DrawFolderItems(folderPath);
                };
                folderListBoxItem.ContextMenu = new ContextMenu
                {
                    Items = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Command = new RelayCommand(async() => {
                                await Commands.CopyTo(folder);

                                if (HasPBO)
                                    _pboPath = await Commands.PackPBO(_editorPath, _pboPath);
                            }),
                            Icon = new DrawingPresenter
                            {
                                Height = 18,
                                Width = 18,
                                Drawing = extractDrawing,
                            },
                            Header = "Copy to"
                        },
                        new MenuItem
                        {
                            Command = new RelayCommand(async() => {
                                await Commands.Delete(folder);
                                DrawFolderItems(_currentPath);

                                if(HasPBO)
                                    _pboPath = await Commands.PackPBO(_editorPath, _pboPath);
                            }),
                            Icon = new DrawingPresenter
                            {
                                Height = 18,
                                Width = 18,
                                Drawing = deleteDrawing,
                            },
                            Header = "Delete"
                        },
                        new MenuItem
                        {
                            Command = new RelayCommand(() =>
                            {
                                EventHandler<RoutedEventArgs> lostFocusEvent = null;
                                EventHandler<RoutedEventArgs> lostFocusEvent2 = null;
                                Action<string> renameFunc = new Action<string>(async (senderText) =>
                                {
                                    // Move the file
                                    string newFileName = senderText;
                                    try {
                                        if (Directory.Exists(Path.Combine(_currentPath, newFileName)))
                                            Directory.Delete(Path.Combine(_currentPath, newFileName), true);

                                        Directory.Move(folderListBoxItem.Tag.ToString(), Path.Combine(_currentPath, newFileName));
                                    }
                                    catch
                                    {
                                        // Reset the text block
                                        ((StackPanel)folderListBoxItem.Content).Children[1] = new TextBlock
                                        {
                                            FontSize = 14,
                                            Foreground = Brushes.White,
                                            Text = Path.GetFileName(folderListBoxItem.Tag.ToString()),
                                            Margin = new Thickness(5, 0, 0, 0),
                                        };
                                        return;
                                    }

                                    // Reset the text block
                                    ((StackPanel)folderListBoxItem.Content).Children[1] = new TextBlock
                                    {
                                        FontSize = 14,
                                        Foreground = Brushes.White,
                                        Text = newFileName,
                                        Margin = new Thickness(5, 0, 0, 0),
                                    };
                                    // Set the new tag
                                    folderListBoxItem.Tag = Path.Combine(_currentPath, newFileName);

                                    if (HasPBO)
                                        // Repack the PBO
                                        _pboPath = await Commands.PackPBO(_editorPath, _pboPath);
                                });

                                TextBox editTextBox = new TextBox
                                {
                                    FontSize = 14,
                                    Foreground = Brushes.White,
                                    Text = Path.GetFileName(folderListBoxItem.Tag.ToString()),
                                    Margin = new Thickness(5, 0, 0, 0),
                                    MinWidth = 200,
                                };
                                editTextBox.KeyUp += (sender, e) =>
                                {
                                    if (e.Key != Avalonia.Input.Key.Enter)
                                        return;

                                    editTextBox.LostFocus -= lostFocusEvent2;
                                    renameFunc.Invoke(((TextBox)sender).Text);
                                };

                                // Set the textbox
                                editTextBox.CaretIndex = Path.GetFileName(folderListBoxItem.Tag.ToString()).Length - Path.GetExtension(folderListBoxItem.Tag.ToString()).Length;
                                ((StackPanel)folderListBoxItem.Content).Children[1] = editTextBox;
                                editTextBox.Focus();

                                lostFocusEvent = (sender, e) => {
                                    editTextBox.Focus();
                                    editTextBox.LostFocus -= lostFocusEvent;

                                    editTextBox.LostFocus += lostFocusEvent2;
                                };
                                lostFocusEvent2 = (sender, e) => {
                                    renameFunc.Invoke(((TextBox)sender).Text);
                                };
                                editTextBox.LostFocus += lostFocusEvent;
                            }),
                            Icon = new DrawingPresenter
                            {
                                Height = 18,
                                Width = 18,
                                Drawing = renameDrawing,
                            },
                            Header = "Rename",
                        },
                    }
                };

                FolderItems.Add(folderListBoxItem);
            });


            DrawFileItems(directoryPath);
        }

        private void DrawFileItems(string directoryPath)
        {
            GeometryDrawing fileDrawing = ((Style)App.Current.Styles.ToArray()[3].Children[0].Children[0]).Resources["Material.File"] as GeometryDrawing;
            GeometryDrawing openDrawing = ((Style)App.Current.Styles.ToArray()[3].Children[0].Children[0]).Resources["Material.OpenInNew"] as GeometryDrawing;
            openDrawing.Brush = Brushes.DeepSkyBlue;
            GeometryDrawing extractDrawing = ((Style)App.Current.Styles.ToArray()[3].Children[0].Children[0]).Resources["Material.PackageDown"] as GeometryDrawing;
            extractDrawing.Brush = Brushes.LimeGreen;
            GeometryDrawing deleteDrawing = ((Style)App.Current.Styles.ToArray()[3].Children[0].Children[0]).Resources["Material.Delete"] as GeometryDrawing;
            deleteDrawing.Brush = Brushes.Red;
            GeometryDrawing renameDrawing = ((Style)App.Current.Styles.ToArray()[3].Children[0].Children[0]).Resources["Material.RenameBox"] as GeometryDrawing;
            renameDrawing.Brush = Brushes.MediumPurple;
            GeometryDrawing checksumDrawing = ((Style)App.Current.Styles.ToArray()[3].Children[0].Children[0]).Resources["Material.Lock"] as GeometryDrawing;
            checksumDrawing.Brush = Brushes.Gold;


            // Display all the files in the current directory
            Directory.GetFiles(directoryPath).ToList().ForEach(file =>
            {
                // Create the file list box item
                ListBoxItem fileListBoxItem = new ListBoxItem
                {
                    Tag = file,
                    Content = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Children =
                        {
                            new DrawingPresenter
                            {
                                Height = 18,
                                Width = 18,
                                Drawing = fileDrawing,
                            },
                            new TextBlock
                            {
                                FontSize = 14,
                                Foreground = Brushes.White,
                                Text = Path.GetFileName(file),
                                Margin = new Thickness(5, 0, 0, 0),
                            }
                        },
                    },
                };
                // Start the file to edit it or do whatever with it...
                fileListBoxItem.DoubleTapped += (sender, e) => {
                    Commands.StartFile(((ListBoxItem)sender).Tag.ToString());
                };
                fileListBoxItem.ContextMenu = new ContextMenu
                {
                    Items = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Command = new RelayCommand(() => Commands.StartFile(file)),
                            Icon = new DrawingPresenter
                            {
                                Height = 18,
                                Width = 18,
                                Drawing = openDrawing,
                            },
                            Header = "Open"
                        },
                        new MenuItem
                        {
                            Command = new RelayCommand(async() => {
                                await Commands.CopyTo(file);

                                if (HasPBO)
                                    _pboPath = await Commands.PackPBO(_editorPath, _pboPath);
                            }),
                            Icon = new DrawingPresenter
                            {
                                Height = 18,
                                Width = 18,
                                Drawing = extractDrawing,
                            },
                            Header = "Copy to"
                        },
                        new MenuItem
                        {
                            Command = new RelayCommand(async() => {
                                await Commands.Delete(file);
                                DrawFolderItems(_currentPath);

                                if (HasPBO)
                                    _pboPath = await Commands.PackPBO(_editorPath, _pboPath);
                            }),
                            Icon = new DrawingPresenter
                            {
                                Height = 18,
                                Width = 18,
                                Drawing = deleteDrawing,
                            },
                            Header = "Delete"
                        },
                        new MenuItem
                        {
                            Command = new RelayCommand(() =>
                            {
                                EventHandler<RoutedEventArgs> lostFocusEvent = null;
                                EventHandler<RoutedEventArgs> lostFocusEvent2 = null;
                                Action<string> renameFunc = new Action<string>(async (senderText) =>
                                {
                                    // Move the file
                                    string newFileName = senderText;
                                    try {
                                        File.Move(fileListBoxItem.Tag.ToString(), Path.Combine(_currentPath, newFileName), true);
                                    }
                                    catch
                                    {
                                        // Reset the text block
                                        ((StackPanel)fileListBoxItem.Content).Children[1] = new TextBlock
                                        {
                                            FontSize = 14,
                                            Foreground = Brushes.White,
                                            Text = Path.GetFileName(fileListBoxItem.Tag.ToString()),
                                            Margin = new Thickness(5, 0, 0, 0),
                                        };
                                        return;
                                    }

                                    // Reset the text block
                                    ((StackPanel)fileListBoxItem.Content).Children[1] = new TextBlock
                                    {
                                        FontSize = 14,
                                        Foreground = Brushes.White,
                                        Text = newFileName,
                                        Margin = new Thickness(5, 0, 0, 0),
                                    };
                                    // Set the new tag
                                    fileListBoxItem.Tag = Path.Combine(_currentPath, newFileName);

                                    if (HasPBO)
                                        // Repack the PBO
                                        _pboPath = await Commands.PackPBO(_editorPath, _pboPath);
                                });

                                TextBox editTextBox = new TextBox
                                {
                                    FontSize = 14,
                                    Foreground = Brushes.White,
                                    Text = Path.GetFileName(fileListBoxItem.Tag.ToString()),
                                    Margin = new Thickness(5, 0, 0, 0),
                                    MinWidth = 200,
                                };
                                editTextBox.KeyUp += (sender, e) =>
                                {
                                    if (e.Key != Avalonia.Input.Key.Enter)
                                        return;

                                    editTextBox.LostFocus -= lostFocusEvent2;
                                    renameFunc.Invoke(((TextBox)sender).Text);
                                };

                                // Set the textbox
                                editTextBox.CaretIndex = Path.GetFileName(fileListBoxItem.Tag.ToString()).Length - Path.GetExtension(fileListBoxItem.Tag.ToString()).Length;
                                ((StackPanel)fileListBoxItem.Content).Children[1] = editTextBox;
                                editTextBox.Focus();

                                lostFocusEvent = (sender, e) => {
                                    editTextBox.Focus();
                                    editTextBox.LostFocus -= lostFocusEvent;

                                    editTextBox.LostFocus += lostFocusEvent2;
                                };
                                lostFocusEvent2 += (sender, e) => {
                                    renameFunc.Invoke(((TextBox)sender).Text);
                                };
                                editTextBox.LostFocus += lostFocusEvent;
                            }),
                            Icon = new DrawingPresenter
                            {
                                Height = 18,
                                Width = 18,
                                Drawing = renameDrawing,
                            },
                            Header = "Rename"
                        },
                        new MenuItem
                        {
                            Command = new RelayCommand(async() => await Commands.Checksum(file)),
                            Icon = new DrawingPresenter
                            {
                                Height = 18,
                                Width = 18,
                                Drawing = checksumDrawing,
                            },
                            Header = "Checksum"
                        },
                    }
                };

                FolderItems.Add(fileListBoxItem);
            });
        }
    }
}
