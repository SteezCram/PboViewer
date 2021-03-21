using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using PboViewer_Lib;
using PboViewer.Core;
using PboViewer.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PboViewer.Views
{
    public class EditorView : UserControl
    {
        public string EditorPath;
        public bool ToDelete = false;

        public EditorView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void Init(string path)
        {
            EditorPath = path;

            if (Path.GetExtension(path) == ".pbo")
            {
                // Set the editor path
                ToDelete = true;
                EditorPath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));
                Directory.CreateDirectory(EditorPath);

                // Pack the folder as a PBO
                using PBOSharpClient pboSharpClient = new PBOSharpClient(path);
                pboSharpClient.ExtractAll(EditorPath);
            }

            DataContext = new EditorViewViewModel(EditorPath, EditorPath != path ? path : null);

            Navigation.NavigationSession.Navigate(EditorPath);
            ((EditorViewViewModel)DataContext).DrawFolderItems(EditorPath);
        }

        /// <summary>
        /// LayoutUpdated event
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBox_LayoutUpdated(object sender, EventArgs e)
        {
            if (!Settings.PboViewerSettings.KeyboardNavigation)
                return;


            ListBox itemListBox = sender as ListBox;

            if (itemListBox.SelectedIndex == -1)
            {
                itemListBox.SelectedIndex = 0;
                IControl listBoxItem = itemListBox.ItemContainerGenerator.ContainerFromIndex(itemListBox.SelectedIndex) as IControl;
                listBoxItem.Focus();
            }
        }

        /// <summary>
        /// KeyUp event
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!Settings.PboViewerSettings.KeyboardNavigation)
                return;


            if (e.Key != Key.Enter)
                return;

            // Get the item path in the tag of the list box item
            string itemPath = ((ListBoxItem)((ListBox)sender).SelectedItem).Tag as string;

            if (File.GetAttributes(itemPath).HasFlag(FileAttributes.Directory)) {
                Navigation.NavigationSession.Navigate(itemPath);
                ((EditorViewViewModel)DataContext).DrawFolderItems(itemPath);
            }
            else
                Commands.StartFile(itemPath);
        }
    }
}
