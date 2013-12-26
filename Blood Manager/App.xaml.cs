using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Threading;

namespace Blood_Manager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const int splashScreenTime = 1300;
        private const int splashFadeTime = 700;

        protected override void OnStartup(StartupEventArgs e)
        {
            SplashScreen splash = new SplashScreen("img/SplashScreen.png");
            splash.Show(false, true);

            Stopwatch timer = new Stopwatch();
            timer.Start();

            base.OnStartup(e); // load main window but don't show yet
            MainWindow main = new MainWindow();

            timer.Stop();
            int remainingTimeForSplash = splashScreenTime - (int)timer.ElapsedMilliseconds;
            if (remainingTimeForSplash > 0)
                Thread.Sleep(remainingTimeForSplash);

            // show main window
            splash.Close(TimeSpan.FromMilliseconds(splashFadeTime));
            main.Show();
        }
    }
}
