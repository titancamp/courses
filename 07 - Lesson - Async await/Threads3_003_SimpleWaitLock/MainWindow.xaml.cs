using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Threads3_003_SimpleWaitLock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SimpleWaitLock simpleWaitLock = new SimpleWaitLock();
        public MainWindow()
        {
            InitializeComponent();
            this.LoadData();
        }

        public async Task LoadData()
        {
            this.MyTextBlock.Text = "Loading data...";

            await Task.Delay(1000);

            this.simpleWaitLock.Enter();

            // Loading Data
            await Task.Delay(2000);
            
            this.simpleWaitLock.Leave();

            this.MyTextBlock.Text = "Data loaded!!!";
        }
    }
}