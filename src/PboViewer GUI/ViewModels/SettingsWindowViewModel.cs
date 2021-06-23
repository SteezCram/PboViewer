using PboViewer.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PboViewer.ViewModels
{
    public class SettingsWindowViewModel : BaseViewModel
    {
        public bool HasSystemIntegration
        {
            get => OperatingSystem.IsWindows();
        }

        public bool KeyboardNavigationChecked
        {
            get => _keyboardNavigationChecked;
            set
            {
                if (value != _keyboardNavigationChecked)
                {
                    _keyboardNavigationChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool PackedChecked
        {
            get => _packedChecked;
            set
            {
                if (value != _packedChecked)
                {
                    _packedChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IntegrationChecked
        {
            get => _integrationChecked;
            set
            {
                if (value != _integrationChecked)
                {
                    _integrationChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool CanModify
        {
            get => _canModify;
            set
            {
                if (value != _canModify)
                {
                    _canModify = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool CannotModify
        {
            get => _cannotModify;
            set
            {
                if (value != _cannotModify)
                {
                    _canModify = value;
                    OnPropertyChanged();
                }
            }
        }


        public ICommand CheckKeyboardNavigation { get; set; }
        public ICommand CheckPacked { get; set; }
        public ICommand CheckIntegration { get; set; }


        private bool _keyboardNavigationChecked;
        private bool _packedChecked;
        private bool _integrationChecked;
        private bool _canModify;
        private bool _cannotModify;


        public SettingsWindowViewModel()
        {
            try {
                Settings.SaveSettings();
                CanModify = true;
                CannotModify = false;
            }
            catch {
                CanModify = false;
                CannotModify = true;
            }

            KeyboardNavigationChecked = Settings.PboViewerSettings.KeyboardNavigation;
            PackedChecked = Settings.PboViewerSettings.OpenPackedPboInFileExplorer;
            IntegrationChecked = Settings.PboViewerSettings.OSIntegration;


            CheckKeyboardNavigation = new RelayCommand(() =>
            {
                // Update the properties
                Settings.PboViewerSettings.KeyboardNavigation = !Settings.PboViewerSettings.KeyboardNavigation;
                PackedChecked = Settings.PboViewerSettings.KeyboardNavigation;

                Settings.SaveSettings();
            });


            CheckPacked = new RelayCommand(() =>
            {
                // Update the properties
                Settings.PboViewerSettings.OpenPackedPboInFileExplorer = !Settings.PboViewerSettings.OpenPackedPboInFileExplorer;
                PackedChecked = Settings.PboViewerSettings.OpenPackedPboInFileExplorer;

                Settings.SaveSettings();
            });

            CheckIntegration = new RelayCommand(() =>
            {
                if (OperatingSystem.IsWindows())
                {
                    if (Settings.PboViewerSettings.OSIntegration)
                    {
                        if (!OS.Windows.Integration.Uninstall())
                            return;

                        Settings.PboViewerSettings.OSIntegration = false;
                        IntegrationChecked = false;
                    }
                    else
                    {
                        if (!OS.Windows.Integration.Install())
                            return;


                        Settings.PboViewerSettings.OSIntegration = true;
                        IntegrationChecked = true;
                    }
                }

                Settings.SaveSettings();
            });
        }
    }
}
