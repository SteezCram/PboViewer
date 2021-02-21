using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PboViewer.ViewModels;

namespace PboViewer.Views
{
    public class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            DataContext = new HomeViewViewModel();
        }
    }
}
