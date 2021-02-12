using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Threads2_011_ConfigureAwait
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var length = this.PreloadData().GetAwaiter().GetResult();

            if (length > 0) {
                MessageBox.Show($"Microsoft page loaded. Data size: {length}");
            }
        }

        private async Task<int> PreloadData()
        {
            var httpClient = new HttpClient();
            var data = await httpClient.GetByteArrayAsync("https://www.microsoft.com/").ConfigureAwait(false);
            
            return data.Length;
        }
        
    }
}