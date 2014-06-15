using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.Threading;

namespace Centapp.Cowbell
{
    public partial class BuyAppPage : PhoneApplicationPage
    {
        public BuyAppPage()
        {
            InitializeComponent();
            Loaded += BuyAppPage_Loaded;
        }

        void BuyAppPage_Loaded(object sender, RoutedEventArgs e)
        {
            StartCountDown();
        }

        private void StartCountDown()
        {
            ((App)Application.Current).WaitIntoBuyPageCompleted = false;
            ButtonBuyApp.IsEnabled = false;
            ButtonNoThanks.IsEnabled = false;

            BackgroundWorker bw = new BackgroundWorker();

            bw.WorkerReportsProgress = true;
            bw.ProgressChanged += bw_ProgressChanged;
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.RunWorkerAsync();
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ButtonBuyApp.IsEnabled = true;
            ButtonNoThanks.IsEnabled = true;

            ((App)Application.Current).WaitIntoBuyPageCompleted = true;
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
                                   {
                                       WaitProgressBar.Value = e.ProgressPercentage;
                                   });
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);
                worker.ReportProgress(i*10);
            }
        }

        private void buyAppButton_Tap(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                var marketplaceDetailTask = new MarketplaceDetailTask();
                marketplaceDetailTask.ContentIdentifier = null;
                marketplaceDetailTask.Show();
            });
        }

        private void noThanksButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}