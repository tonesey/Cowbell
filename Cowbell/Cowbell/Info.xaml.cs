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

namespace DeveloperExcuses
{
    public partial class Info : PhoneApplicationPage
    {
        public Info()
        {
            InitializeComponent();
            
            var appEl = XDocument.Load("WMAppManifest.xml").Root.Element("App");
            string appName = appEl.Attribute("Title").Value;
            var ver = new Version(appEl.Attribute("Version").Value);
            string appVersion = string.Format("{0}.{1}", ver.Major, ver.Minor);
            ApplicationTitle.Text = string.Format("{0} v{1}", appName, appVersion);

            //InfoTextBlock.Text = AppResources.Infos;
            //RateItButton.Content = AppResources.RateAppButtonText;
            //ReleaseNotesTextblock.Text = AppResources.ReleaseNotes;
        }

        //private void Email_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        //{
        //    EmailComposeTask email = new EmailComposeTask();
        //    email.To = "centapp@hotmail.com";
        //    email.Subject = AppResources.RateEmailSubject;
        //    email.Show();
        //}

        private void RateItButton_Tap(object sender, GestureEventArgs e)
        {
            MarketplaceReviewTask task = new MarketplaceReviewTask();
            task.Show();
        }
    }
}