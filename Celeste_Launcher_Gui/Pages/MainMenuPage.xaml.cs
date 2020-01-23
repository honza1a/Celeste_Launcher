﻿using Celeste_Launcher_Gui.Services;
using Celeste_Launcher_Gui.Windows;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Celeste_Launcher_Gui.Pages
{
    /// <summary>
    /// Interaction logic for MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page
    {
        public MainMenuPage()
        {
            InitializeComponent();
        }

        #region Navigation
        private void OnLoginClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Pages/LoginPage.xaml", UriKind.Relative));
        }

        private void OnRegisterClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Pages/RegisterPage.xaml", UriKind.Relative));
        }

        private async void OnPlayOfflineClick(object sender, RoutedEventArgs e)
        {
            PlayOfflineBtn.IsEnabled = false;
            await GameService.StartGame(true);
            PlayOfflineBtn.IsEnabled = true;
        }
        #endregion

        #region Links
        private void OnCelesteClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://projectceleste.com");
        }

        private void OnDiscordClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://discord.gg/xXFUvWA");
        }

        private void OnRedditClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.reddit.com/r/projectceleste/");
        }

        private void OnYoutubeClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.youtube.com/channel/UC5gS69ffCAeqvJrqWvlnZgg");
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            SettingsButton.ContextMenu.PlacementTarget = sender as UIElement;
            SettingsButton.ContextMenu.IsOpen = true;
        }

        private void OnDonateClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://forums.projectceleste.com/donations/project-celeste-donations.2/campaign");
        }
        #endregion

        #region Settings
        private void OpenWindowsFeatures(object sender, RoutedEventArgs e)
        {
            var windowsFeatureHelper = new WindowsFeatureHelper
            {
                Owner = Window.GetWindow(this)
            };
            windowsFeatureHelper.ShowDialog();
        }

        private void EnableDiagnosticsMode(object sender, RoutedEventArgs e)
        {
            LegacyBootstrapper.UserConfig.IsDiagnosticMode = !LegacyBootstrapper.UserConfig.IsDiagnosticMode;

            if (!LegacyBootstrapper.UserConfig.IsDiagnosticMode)
                return;

            try
            {
                var procdumpFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "procdump.exe");
                if (File.Exists(procdumpFileName))
                    return;
                var dialog = new GenericMessageDialog(Properties.Resources.EnableDiagnosticsModeInstallProcdumpPrompt,
                    DialogIcon.Warning, DialogOptions.YesNo)
                { Owner = Window.GetWindow(this) };

                var dr = dialog.ShowDialog();
                if (dr != null && dr.Value)
                {
                    var procDumpInstallerDialog = new ProcDumpInstaller
                    {
                        Owner = Window.GetWindow(this)
                    };
                    procDumpInstallerDialog.ShowDialog();
                }
                else
                {
                    LegacyBootstrapper.UserConfig.IsDiagnosticMode = false;
                }
            }
            catch (Exception exception)
            {
                LegacyBootstrapper.UserConfig.IsDiagnosticMode = false;
                GenericMessageDialog.Show($"{Properties.Resources.EnableDiagnosticsModeProcdumpInstallError} {exception.Message}", DialogIcon.Warning);
            }
        }

        private void OpenSteam(object sender, RoutedEventArgs e)
        {
            var steamConverterWindow = new SteamConverterWindow
            {
                Owner = Window.GetWindow(this)
            };
            steamConverterWindow.ShowDialog();
        }

        private void OpenGameLanguage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Pages/LanguageSelectionPage.xaml", UriKind.Relative));
        }

        private void OpenGameScanner(object sender, RoutedEventArgs e)
        {
            var pname = Process.GetProcessesByName("spartan");
            if (pname.Length > 0)
            {
                GenericMessageDialog.Show(Properties.Resources.GameAlreadyRunningError, DialogIcon.Error);
                return;
            }

            var scanner = new GamePathSelectionWindow
            {
                Owner = Window.GetWindow(this)
            };
            scanner.ShowDialog();
        }

        private void OpenFirewallHelper(object sender, RoutedEventArgs e)
        {
            var firewallHelper = new Windows.WindowsFirewallHelper
            {
                Owner = Window.GetWindow(this)
            };
            firewallHelper.ShowDialog();
        }

        private void OpenLauncherUpdater(object sender, RoutedEventArgs e)
        {
            var pname = Process.GetProcessesByName("spartan");
            if (pname.Length > 0)
            {
                GenericMessageDialog.Show(Properties.Resources.GameAlreadyRunningError, DialogIcon.Error);
                return;
            }

            var updater = new UpdateWindow
            {
                Owner = Window.GetWindow(this)
            };
            updater.ShowDialog();
        }
        #endregion
    }
}
