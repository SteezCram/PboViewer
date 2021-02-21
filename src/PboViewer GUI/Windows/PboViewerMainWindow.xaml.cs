using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PboViewer.ViewModels;
using PboViewer.Views;
using System.IO;

namespace PboViewer.Windows
{
    /// <summary>
    /// Main window of PBO Viewer
    /// </summary>
    public class PboViewerMainWindow : FluentWindow
    {
        public TextBlock Title;

        public PboViewerMainWindow()
        {
            InitializeComponent();
#if DEBUG
            //this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            Closing += PboViewerMainWindow_Closing;
            Title = this.FindControl<TextBlock>("Title");

            DataContext = new PboViewerMainWindowViewModel();
        }

        private void PboViewerMainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            EditorView editorView = ((EditorView)((PboViewerMainWindowViewModel)DataContext).Child);
            if (editorView.ToDelete)
                Directory.Delete(editorView.EditorPath, true);
        }
    }
}
