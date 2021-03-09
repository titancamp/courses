using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Threads3_002_SingleInstance
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// <remarks>
    /// Here we need to allow only one app to run at a time.
    /// It should be a single instance app, how will you do that?
    /// </remarks>
    public partial class App : Application
    {
        private Semaphore semaphore;
        protected override void OnStartup(StartupEventArgs e)
        {
            this.semaphore = new Semaphore(0, 1, "SingleInstanceApp_001", out var isCreated);

            if (!isCreated) {
                MessageBox.Show("Another app is already running, try again later.");
                Current.Shutdown();
            }

            base.OnStartup(e);
        }
    }
}