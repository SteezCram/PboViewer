using Avalonia.Controls;
using PboViewer.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PboViewer.ViewModels
{
    public class HomeViewViewModel : BaseViewModel
    {
        public ICommand OpenFolder { get; set; }
        public ICommand OpenPBO { get; set; }


        public HomeViewViewModel()
        {
            OpenFolder = new RelayCommand(() => Commands.OpenFolder());
            OpenPBO = new RelayCommand(() => Commands.OpenPBO());
        }
    }
}
