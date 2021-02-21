using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using PBOSharp;
using PboViewer.ViewModels;
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
                ToDelete = true;
                EditorPath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));
                Directory.CreateDirectory(EditorPath);

                // Pack the folder as a PBO
                using PBOSharpClient pboSharpClient = new PBOSharpClient();
                pboSharpClient.ExtractAll(path, EditorPath);

                Debug.WriteLine(EditorPath);
            }

            DataContext = new EditorViewViewModel(EditorPath, EditorPath != path ? path : null);
            ((EditorViewViewModel)DataContext).DrawFolderItems(EditorPath);
        }
    }
}
