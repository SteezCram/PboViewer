using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PboViewer.ViewModels;

namespace PboViewer
{
    public class PboViewerMainWindow : Window
    {
        public PboViewerMainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            DataContext = new PboViewerMainWindowViewModel();
        }
    }
}
