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
using System.Xml.Linq;
using Microsoft.Phone.Tasks;

namespace Centapp.Cowbell
{
    public partial class Info : PhoneApplicationPage
    {

        string _appName = string.Empty;
        string _appVersion = string.Empty;

        public Info()
        {
            InitializeComponent();
            var appEl = XDocument.Load("WMAppManifest.xml").Root.Element("App");
            var ver = new Version(appEl.Attribute("Version").Value);
            _appName = appEl.Attribute("Title").Value.ToUpper();
            _appVersion = string.Format("{0}.{1}", ver.Major, ver.Minor);
            Loaded += Info_Loaded;
        }

        void Info_Loaded(object sender, RoutedEventArgs e)
        {
            InitCaptions();
        }

        private void InitCaptions()
        {
            InfoTextblock.Text = AppResources.InfoPageInfoTextblock;
            ApplicationTitle.Text = string.Format("{0} v{1}", _appName, _appVersion);
        }

        private void Email_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EmailComposeTask email = new EmailComposeTask();
            email.To = "centapp@hotmail.com";
            email.Subject = _appName;
            email.Show();
        }

        //private void RateItButton_Tap(object sender, GestureEventArgs e)
        //{
        //    MarketplaceReviewTask task = new MarketplaceReviewTask();
        //    task.Show();
        //}
    }
}