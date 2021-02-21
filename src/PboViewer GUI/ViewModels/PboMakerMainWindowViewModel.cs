using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using PBOSharp;
using PBOSharp.Objects;
using PboViewer.Core;
using PboViewer.Views;
//using PboViewer.Core;
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

        private UserControl _child;


        public PboViewerMainWindowViewModel()
        {
            Child = new HomeView();
        }
    }
}
