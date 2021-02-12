using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Threads2_003_Cancellation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly CancellationTokenSource cts;
        private CancellationTokenSource cts2;
        public MainWindow()
        {
            InitializeComponent();

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                Dispatcher.Invoke(() =>
                {
                    this.MyTextBlock.Text = $"UnobservedTaskException: {args.Exception}";
                });
            };

            this.MyTextBlock.Text = $"Running on Thread: {Thread.CurrentThread.ManagedThreadId}";

            cts = new CancellationTokenSource();
            var token = cts.Token;
            token.Register(() =>
            {
                var context = SynchronizationContext.Current;
                var threadId = Thread.CurrentThread.ManagedThreadId;
                this.MyTextBlock.Text = $"Cancelled from Thread: {threadId}, sync context: {context}";
            }, false);

            ThreadPool.QueueUserWorkItem(async state => await EditTextMessage(token));

            Task.Run(() => throw new Exception("Try to Catch Me"));
        }

        private async Task EditTextMessage(CancellationToken token)
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            await Task.Delay(3000);

            if (token.IsCancellationRequested) {
                Dispatcher.Invoke(() =>
                {
                    this.MyTextBlock.Text = $"Cancellation requested on: {threadId}";
                });
                return;
            }
            
            cts2 = new CancellationTokenSource();
            var innerToken = cts2.Token;
            innerToken.Register(() =>
            {
                var innerContext = SynchronizationContext.Current;
                var innerThreadId = Thread.CurrentThread.ManagedThreadId;
                this.MyTextBlock.Text = $"Cancelled from Thread: {innerThreadId}, sync context: {innerContext}";
            }, false);
                
            ThreadPool.QueueUserWorkItem(async state => await this.EditFromThreadPool(innerToken));

            Dispatcher.Invoke(() =>
            {
                this.MyTextBlock.Text = $"First delay ended: {threadId}";
            });

            await Task.Delay(3000);
                    
            Dispatcher.Invoke(() =>
            {
                this.MyTextBlock.Text = $"Editing from Thread: {threadId}";
            });
        }

        private async Task EditFromThreadPool(CancellationToken token)
        {
            var innerContext = SynchronizationContext.Current;
            var innerThreadId = Thread.CurrentThread.ManagedThreadId;
            
            Dispatcher.Invoke(() =>
            {
                this.MyTextBlock.Text = $"Editing from Thread Pool Thread: {innerThreadId}, context: {innerContext}";
            });
            
            await Task.Delay(3000);

            if (token.IsCancellationRequested) {
                Dispatcher.Invoke(() =>
                {
                    this.MyTextBlock.Text = $"Cancellation from Thread Pool: {innerThreadId}, context: {innerContext}";
                });
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.cts.Cancel();
        }
        
        private void ButtonBasePool_OnClick(object sender, RoutedEventArgs e)
        {
            this.cts2.Cancel();
        }
    }
}