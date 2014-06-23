using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Centapp.Cowbell;
using Centapp.Cowbell.Helpers;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Audio;
using Wp7Shared.Sensors;
using Microsoft.Phone.Shell;
using System.Resources;
using System.Globalization;
using System.Threading;
using Wp8Shared.Helpers;


namespace Centapp.Cowbell
{
    public partial class MainPage : PhoneApplicationPage
    {

        ShakeDetector _sd = null;
        Storyboard _sbBell = null;
        Storyboard _sbText = null;

        private int _shakesCounter = 0;

        private const string _buyapppageXaml = "/BuyAppPage.xaml";
        private const string _infopageXaml = "/Info.xaml";

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            CultureInfo cc, cuic;
            cc = Thread.CurrentThread.CurrentCulture;
            cuic = Thread.CurrentThread.CurrentUICulture;

            //Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES");

            _sbBell = (Storyboard)Resources["BellAnimationSb"];
            _sbText = (Storyboard)Resources["TextAnimationSb"];
            Loaded += MainPage_Loaded;

            ((App)Application.Current).BackFromBuyPage = false;

            BuildApplicationBar();
        }

        private void BuildApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.

            if (ApplicationBar == null)
            {
                ApplicationBar = new ApplicationBar();

                var btnInfo = new ApplicationBarIconButton(new Uri("/Resources/appbar/info.png", UriKind.Relative));
                btnInfo.Text = AppResources.AppBarButtonInfoText;
                btnInfo.Click += AppBarButtonInfo_OnClick;
                ApplicationBar.Buttons.Add(btnInfo);

                // Create a new button and set the text value to the localized string from AppResources.
                var btnRate = new ApplicationBarIconButton(new Uri("/Resources/appbar/like.png", UriKind.Relative));
                btnRate.Text = AppResources.AppBarButtonRateText;
                btnRate.Click += AppBarButtonRate_OnClick;
                ApplicationBar.Buttons.Add(btnRate);

                ApplicationBar.Mode = ApplicationBarMode.Minimized;
            }
        }

        void btnRate_Click(object sender, EventArgs e)
        {
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            InitCaptions();
            InitTextInfo();
        }

        private void InitCaptions()
        {
            TextBlockInfos.Text = AppResources.MainPageTextBoxInfoText;

        }

        private void InitTextInfo()
        {
            _sbText.Stop();
            _sbText.Completed += (s, e1) =>
            {
                _sbText.Stop();
                InitSensor();
            };
            _sbText.Begin();
        }

        //private void LoadData(string culture)
        //{
        //    string subfolder = "default";
        //    if (culture.Equals("it-IT"))
        //    {
        //        subfolder = "it";
        //    }
        //    Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("DeveloperExcuses.Resources.{0}.sentences.txt", subfolder));
        //    StreamReader streamReader = new StreamReader(stream);
        //    string fileContent = streamReader.ReadToEnd();
        //    _sentences = new List<string>(fileContent.Split('\n'));
        //}

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        private void InitSensor()
        {
            _sd = new ShakeDetector();
            _sd.ShakeEvent -= _sd_ShakeEvent;
            _sd.ShakeEvent += _sd_ShakeEvent;
            _sd.Start();
        }

        private void _sd_ShakeEvent(object sender, EventArgs e)
        {
            _shakesCounter++;
            Dispatcher.BeginInvoke(() =>
                                   {

                                       if (((App)Application.Current).IsTrial && _shakesCounter > 20)
                                       {
                                           GotoBuyPage();
                                           return;
                                       }

                                       //if (BorderTextInfo.Visibility != Visibility.Collapsed)
                                       {
                                           _sbText.Stop();
                                           //BorderTextInfo.Visibility = Visibility.Collapsed;
                                       }

                                       //Microsoft.Devices.VibrateController.Default.Start(TimeSpan.FromSeconds(0.2));

                                       var num = RandomNumber(1, (Application.Current as App).Sounds.Keys.Count + 1);
                                       SoundEffect soundEffect = (Application.Current as App).Sounds[num];
                                       SoundHelper.PlaySound(soundEffect);
                                       _sbBell.Stop();
                                       _sbBell.Completed += (s, e1) =>
                                       {
                                           _sbBell.Stop();
                                       };
                                       _sbBell.Begin();
                                   });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            base.OnNavigatedTo(e);

            bool userNavigatedAwayFromBuyPage = false;
            //var lastPage = NavigationService.BackStack.FirstOrDefault();
            //if (lastPage != null && lastPage.Source.ToString() == _buyapppageXaml)
            //{
            //    userNavigatedAwayFromBuyPage = true;
            //}

            userNavigatedAwayFromBuyPage = (e.NavigationMode == NavigationMode.Back) && ((App)Application.Current).IsTrial && ((App)Application.Current).BackFromBuyPage;

            if (userNavigatedAwayFromBuyPage)
            {
                ((App)Application.Current).BackFromBuyPage = false;

                if (!((App)Application.Current).WaitIntoBuyPageCompleted)
                {
                    //l'utente ha fatto back senza attendere il completamento dell'attesa nella versione trial
                    GotoBuyPage();
                }
                else
                {
                    _shakesCounter = 0;
                }
            }
        }

        private void GotoBuyPage()
        {
            NavigationService.Navigate(new Uri(_buyapppageXaml, UriKind.Relative));
        }

        private void AppBarButtonInfo_OnClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(_infopageXaml, UriKind.Relative));
        }

        private void AppBarButtonRate_OnClick(object sender, EventArgs e)
        {
            FeedbackHelper.Default.Reviewed();
            var marketplace = new MarketplaceReviewTask();
            marketplace.Show();
        }
    }
}