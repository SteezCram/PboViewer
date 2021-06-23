using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using PboViewer_Lib;
using PboViewer_Lib.Objects;
using PboViewer.Core;
using PboViewer.Views;
using PboViewer.Windows;
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
        public UserControl Child
        {
            get => _child;
            set
            {
                if (value != _child)
                {
                    _child = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand Home { get; set; }
        public ICommand OpenSettings { get; set; }

        private UserControl _child;


        public PboViewerMainWindowViewModel()
        {
            Child = new HomeView();

            Home = new RelayCommand(() => {
                // Change the title
                ((IClassicDesktopStyleApplicationLifetime)App.Current.ApplicationLifetime).MainWindow.Title = "PBO Viewer";
                ((PboViewerMainWindow)((IClassicDesktopStyleApplicationLifetime)App.Current.ApplicationLifetime).MainWindow).Title.Text = "PBO Viewer";

                Child = new HomeView();
            });
            OpenSettings = new RelayCommand(() => new SettingsWindow().ShowDialog(((IClassicDesktopStyleApplicationLifetime)App.Current.ApplicationLifetime).MainWindow));
        }
    }
}
