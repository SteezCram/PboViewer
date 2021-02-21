using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PboViewer.ViewModels;

namespace PboViewer.Windows
{
    public class ChecksumWindow : FluentWindow
    {
        public ChecksumWindow()
        {
            InitializeComponent();
#if DEBUG
            //this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void Init(string path, string checksum)
        {
            DataContext = new ChecksumWindowViewModel
            {
                File = path,
                SHA = "SHA 256 checksum:",
                ChecksumResult = $"Checksum result for:",
                Checksum = checksum,
            };

            Show();
        }
    }
}
