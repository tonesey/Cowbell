using System;
using System.Collections.Generic;
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
using System.Xml.Linq;
using System.Windows.Media.Imaging;

namespace Centapp.Memessenger
{
    public partial class InfoPage : PhoneApplicationPage
    {
        string _appVer = string.Empty;

        public InfoPage()
        {
            InitializeComponent();

            var appEl = XDocument.Load("WMAppManifest.xml").Root.Element("App");
            var ver = new Version(appEl.Attribute("Version").Value);
            _appVer = string.Format("{0}.{1}", ver.Major, ver.Minor);
        }

        private void contactButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var email = new EmailComposeTask();
            email.To = "centapp@hotmail.com";
            email.Subject = LocalizedResources.email_feedback_subject;
            email.Show();
        }

        private void rateButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MarketplaceReviewTask task = new MarketplaceReviewTask();
            task.Show();
        }

        private void buyAppButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                var marketplaceDetailTask = new MarketplaceDetailTask();
                marketplaceDetailTask.ContentIdentifier = null;
                marketplaceDetailTask.Show();
            });
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            buyAppButton.Content = LocalizedResources.btnBuy;
            buyAppButton.Visibility = ((App)Application.Current).IsTrial ? Visibility.Visible : Visibility.Collapsed;
            buyAppButtonHelp.Visibility = buyAppButton.Visibility;
            buyAppButtonHelp.Text = LocalizedResources.buyAppButtonHelp;

            rateButton.Content = LocalizedResources.btnRate;
            rateButtonHelp.Text = LocalizedResources.rateButtonHelp;

            contactButton.Content = LocalizedResources.btnContactUs;
            contactButtonHelp.Text = LocalizedResources.contactButtonHelp;

            UpdateAppInfos();
        }

        private void UpdateAppInfos()
        {
            infoImage.Source = new BitmapImage(new Uri(App.ViewModel.Memes.ElementAt(RandomNumber(0, App.ViewModel.Memes.Count - 1)).PictureUri, UriKind.Relative));

            string trialOrReg = ((App)Application.Current).IsTrial ? LocalizedResources.trial : LocalizedResources.full;
            appTitleTextblock.Text = string.Format(LocalizedResources.appVersion, new string[] { _appVer });
            licenseTextblock.Text = trialOrReg;
          //  licenseTextblock.Foreground = ((App)Application.Current).IsTrial ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Green);
        }

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}