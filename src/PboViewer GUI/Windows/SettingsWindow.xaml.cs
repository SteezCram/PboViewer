using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PboViewer.ViewModels;

namespace PboViewer.Windows
{
    public class SettingsWindow : FluentWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();
#if DEBUG
            //this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            DataContext = new SettingsWindowViewModel();
        }
    }
}
