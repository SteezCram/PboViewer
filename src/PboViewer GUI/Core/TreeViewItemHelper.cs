using Avalonia.Controls;
using PBOSharp;
using PBOSharp.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace PboViewer.Core
{
    class TreeViewItemHelper
    {
        /// <summary>
        /// Generate the PBO tree view
        /// </summary>
        //private void GenerateTreeView()
        //{
        //    // Get the treeview
        //    TreeView mainWindowTreeView = _mainWindow.Find<TreeView>("PboFileTreeView");

        //    TreeViewItem baseTreeViewItem = new TreeViewItem
        //    {
        //        Header = new StackPanel
        //        {
        //            Orientation = Avalonia.Layout.Orientation.Horizontal,
        //            Children =
        //            {
        //                new TextBlock
        //                {
        //                    FontSize = 14,
        //                    Foreground = Brushes.White,
        //                    Text = Path.GetFileName(PboFileName),
        //                }
        //            },
        //        }
        //    };

        //    //baseTreeViewItem.DoubleTapped += (sender, e) =>
        //    //{
        //    //    string? treeViewItemDirectoryPath = (string)((TreeViewItem)sender).Tag;

        //    //    // Analyse the PBO
        //    //    PBOSharpClient pboSharpClient = new PBOSharpClient();
        //    //    PBO loadedPBO = pboSharpClient.AnalyzePBO(PboFileName);

        //    //    int filesCount = loadedPBO.Files.Count;
        //    //    string currentFolder = "";

        //    //    for (int i = 0; i < filesCount; i++)
        //    //    {
        //    //        PBOFile currentPboFile = loadedPBO.Files[i];

        //    //        if (currentPboFile.)
        //    //    }

        //    //    pboSharpClient.Dispose();
        //    //};

        //    return List<TreeViewItem> treeViewItems = GetPboItem();
        //    treeViewItems.ForEach(x => PboTreeViewItems.Add(x));
        //}

        /// <summary>
        /// Get all the PBO items
        /// </summary>
        //private List<TreeViewItem> GetPboItem(string pboFileName)
        //{
        //    List<TreeViewItem> treeViewItems = new List<TreeViewItem>();

        //    // Analyse the PBO
        //    PBOSharpClient pboSharpClient = new PBOSharpClient();
        //    PBO loadedPBO = pboSharpClient.AnalyzePBO(pboFileName);
        //    TreeViewItem currentFolderTreeViewItem = null;

        //    int filesCount = loadedPBO.Files.Count;
        //    string currentFolder = "";

        //    for (int i = 0; i < filesCount; i++)
        //    {
        //        PBOFile currentPboFile = loadedPBO.Files[i];

        //        while (Path.GetDirectoryName(currentPboFile.FileName) == currentFolder)
        //        {
        //            treeViewItems.Add(new TreeViewItem
        //            {
        //                Header = new StackPanel
        //                {
        //                    Orientation = Avalonia.Layout.Orientation.Horizontal,
        //                    Children = {
        //                        new TextBlock
        //                        {
        //                            FontSize = 14,
        //                            Foreground = Brushes.White,
        //                            Text = currentPboFile.FileNameShort,
        //                        }
        //                    },
        //                },
        //            });

        //            i++;
        //        }

        //        currentFolder = Path.GetDirectoryName(currentPboFile.FileName);
        //        currentFolderTreeViewItem = new TreeViewItem
        //        {
        //            Header = new StackPanel
        //            {
        //                Orientation = Avalonia.Layout.Orientation.Horizontal,
        //                Children = {
        //                    new TextBlock {
        //                        FontSize = 14,
        //                        Foreground = Brushes.White,
        //                        Text = currentFolder,
        //                    }
        //                },
        //            },
        //        };

        //        //if (Path.GetDirectoryName(currentPboFile.FileName) != currentFolder)
        //        //{
        //        //    treeViewItems.Add(new TreeViewItem
        //        //    {
        //        //        Header = new StackPanel
        //        //        {
        //        //            Orientation = Avalonia.Layout.Orientation.Horizontal,
        //        //            Children =
        //        //            {
        //        //                new TextBlock
        //        //                {
        //        //                    FontSize = 14,
        //        //                    Foreground = Brushes.White,
        //        //                    Text = Path.GetFileName(PboFileName),
        //        //                }
        //        //            },
        //        //        },
        //        //        Items = GetPboItem(Path.GetDirectoryName(currentPboFile.FileName))
        //        //    });
        //        //}
        //        //else
        //        //{
        //        //    treeViewItems.Add(new TreeViewItem
        //        //    {
        //        //        Header = new StackPanel
        //        //        {
        //        //            Orientation = Avalonia.Layout.Orientation.Horizontal,
        //        //            Children =
        //        //            {
        //        //                new TextBlock
        //        //                {
        //        //                    FontSize = 14,
        //        //                    Foreground = Brushes.White,
        //        //                    Text = currentPboFile.FileNameShort,
        //        //                }
        //        //            },
        //        //        },
        //        //    });
        //        //}
        //    }

        //    pboSharpClient.Dispose();

        //    return treeViewItems;
        //}
    }
}
