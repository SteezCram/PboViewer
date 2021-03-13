using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using PboViewer.Core;
using System.IO;
using System.Threading.Tasks;

namespace PboViewer.Windows
{
    public class ProgressWindow : FluentWindow
    {
        new public TextBlock Title;
        public TextBlock OperationTextBlock;
        public TextBlock OperationDetailTextBlock;

        public ProgressWindow()
        {
            InitializeComponent();
#if DEBUG
            //this.AttachDevTools();
#endif
        }

        public async void Operation(string operation, string itemPath, string destinationPath = null)
        {
            if (operation[1] == 'e')
            {
                Title.Text = "PBO Viewer - Extracting";
                OperationTextBlock.Text = $"Extracting...";
                OperationDetailTextBlock.Text = $"{itemPath} to {Path.Combine(destinationPath, Path.GetFileNameWithoutExtension(itemPath))}";

                await Task.Run(() => Commands.UnpackPBO(itemPath, destinationPath));
            }
            else
            {
                Title.Text = "PBO Viewer - Packing";
                OperationTextBlock.Text = $"Packing...";
                OperationDetailTextBlock.Text = $"{itemPath} to {destinationPath}";

                await Task.Run(() => Commands.PackPBO(itemPath, destinationPath));
            }


            ((IClassicDesktopStyleApplicationLifetime)App.Current.ApplicationLifetime).Shutdown();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            Title = this.FindControl<TextBlock>("Title");
            OperationTextBlock = this.FindControl<TextBlock>("OperationTextBlock");
            OperationDetailTextBlock = this.FindControl<TextBlock>("OperationDetailTextBlock");
        }
    }
}
