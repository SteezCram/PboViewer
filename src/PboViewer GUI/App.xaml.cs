﻿using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using PboViewer.Core;
using PboViewer.OS.Windows;
using PboViewer.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace PboViewer
{
    public class App : Application
    {
        [STAThread]
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Settings.LoadSettings();
                Navigation.NavigationSession = new Navigation();

                switch (desktop.Args.Length)
                {
                    case 1:
                        desktop.MainWindow = new PboViewerMainWindow();
                        desktop.MainWindow.Show();

                        // Open the file
                        if (Path.GetExtension(desktop.Args[0]) == ".pbo")
                            Commands.OpenPBO(desktop.Args[0]);
                        // Open the folder
                        else if (Path.GetExtension(desktop.Args[0]) == "")
                            Commands.OpenFolder(desktop.Args[0]);

                        base.OnFrameworkInitializationCompleted();
                        break;

                    case 2:
                        ProgressWindow progressWindow = new ProgressWindow();
                        desktop.MainWindow = progressWindow;
                        desktop.MainWindow.Show();

                        switch (desktop.Args[1])
                        {
                            // Extract here
                            case "-eh":
                                progressWindow.Operation(desktop.Args[1], desktop.Args[0], Path.GetDirectoryName(desktop.Args[0]));
                                break;

                            // Extract to
                            case "-et":
                                progressWindow.Operation(desktop.Args[1], desktop.Args[0]);
                                break;

                            // Pack here
                            case "-ph":
                                progressWindow.Operation(desktop.Args[1], desktop.Args[0], Path.Combine(Path.GetDirectoryName(desktop.Args[0]), $"{Path.GetFileName(desktop.Args[0])}.pbo"));
                                break;

                            // Pack to
                            case "-pt":
                                progressWindow.Operation(desktop.Args[1], desktop.Args[0]);
                                break;
                        }

                        base.OnFrameworkInitializationCompleted();
                        break;

                    default:
                        desktop.MainWindow = new PboViewerMainWindow();
                        desktop.MainWindow.Show();
                        break;
                }
            }
        }
    }
}
