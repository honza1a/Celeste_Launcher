﻿#region Using directives

using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Celeste_Launcher_Gui.Forms;
using Celeste_Launcher_Gui.Helpers;

#endregion

namespace Celeste_Launcher_Gui
{
    internal static class Program
    {
#if DEBUG
        public static string WebSocketUri = "ws://127.0.0.1:4512/";
#else
        public static string WebSocketUri = "ws://66.70.180.188:4512/";
#endif

        public static WebSocketClient WebSocketClient = new WebSocketClient(WebSocketUri);
        public static UserConfig UserConfig = new UserConfig();
        public static string UserConfigFilePath = $"{AppDomain.CurrentDomain.BaseDirectory}CelesteConfig.xml";

        private const string AppName = "CelesteFanProject";

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // ReSharper disable once UnusedVariable
            var mutex = new Mutex(true, AppName, out bool createdNew);

            //Only one instance
            if (!createdNew)
            {
                SkinHelper.ShowMessage(@"Celeste Fan Project launcher already runing!", "Celeste Fan Project",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            //Load UserConfig
            try
            {
                if (File.Exists(UserConfigFilePath))
                    UserConfig = UserConfig.Load(UserConfigFilePath);
            }
            catch (Exception)
            {
                //
            }

            //Start Gui
            Application.Run(new MainForm());
        }
    }
}