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

        public void Init(string path, string shaChecksum, string md5Checksum)
        {
            DataContext = new ChecksumWindowViewModel
            {
                File = path,
                SHA = "SHA 256 checksum:",
                MD5 = "MD5 checksum:",
                ChecksumResult = $"Checksum result for:",
                SHAChecksum = shaChecksum,
                MD5Checksum = md5Checksum,
            };

            Show();
        }
    }
}
