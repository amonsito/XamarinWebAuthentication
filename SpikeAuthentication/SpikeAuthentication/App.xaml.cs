using MonkeyCache.FileStore;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpikeAuthentication
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Barrel.ApplicationId = "com.Spike.Authentication";
            MainPage = new NavigationPage(new LoginPageTest());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
