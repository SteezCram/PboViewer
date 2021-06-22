using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using PboViewer.Core;
using PboViewer.ViewModels;
using PboViewer.Views;
using System;
using System.Diagnostics;
using System.IO;

namespace PboViewer.Windows
{
    /// <summary>
    /// Main window of PBO Viewer
    /// </summary>
    public class PboViewerMainWindow : FluentWindow
    {
        new public TextBlock Title;

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

            // Modify the button place
            if (!OperatingSystem.IsWindows())
                this.FindControl<Button>("SettingsButton").Margin = new Thickness(0);

            // Register general event
            Closing += PboViewerMainWindow_Closing;
            this.AddHandler(PointerReleasedEvent, PboViewerMainWindow_MouseUp, handledEventsToo: true);

            // Set properties
            Title = this.FindControl<TextBlock>("Title");
            DataContext = new PboViewerMainWindowViewModel();
        }

        /// <summary>
        /// Window closing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PboViewerMainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            IO.Unwatch();

            if (((PboViewerMainWindowViewModel)DataContext).Child is not EditorView)
                return;

            // Delete the extracted PBO
            EditorView editorView = ((EditorView)((PboViewerMainWindowViewModel)DataContext).Child);
            if (editorView.ToDelete)
                Directory.Delete(editorView.EditorPath, true);
        }

        private void PboViewerMainWindow_MouseUp(object sender, PointerReleasedEventArgs e)
        {
            PointerUpdateKind pointerUpdateKind = e.GetCurrentPoint(this).Properties.PointerUpdateKind;
            Debug.WriteLine(Enum.GetName(pointerUpdateKind.GetType(), pointerUpdateKind));

            if (((PboViewerMainWindowViewModel)DataContext).Child is not EditorView)
                return;

            if (pointerUpdateKind == PointerUpdateKind.XButton1Released)
            {
                Navigation.NavigationSession.NavigateBack();
                EditorView editorView = ((PboViewerMainWindowViewModel)DataContext).Child as EditorView;

                ((EditorViewViewModel)editorView.DataContext).DrawFolderItems(Navigation.NavigationSession.Item as string);
            }
            else if (pointerUpdateKind == PointerUpdateKind.XButton2Released)
            {
                Navigation.NavigationSession.Navigate();
                EditorView editorView = ((PboViewerMainWindowViewModel)DataContext).Child as EditorView;

                ((EditorViewViewModel)editorView.DataContext).DrawFolderItems(Navigation.NavigationSession.Item as string);
            }
            //PointerUpdateKind pointerUpdateKind = e.Pr
            //if (e. == PointerUpdateKind.LeftButtonPressed || kind == PointerUpdateKind.LeftButtonReleased)
        }
    }
}
