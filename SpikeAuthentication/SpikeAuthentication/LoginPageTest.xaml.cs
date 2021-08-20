using MonkeyCache.FileStore;
using Newtonsoft.Json;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using SpikeAuthentication.Dto;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpikeAuthentication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPageTest : ContentPage
    {
        const string UserBiometric = "UserBiometric";
        public LoginPageTest()
        {

            InitializeComponent();
            MessagingCenter.Subscribe<MainPage>(this, "Hi", (sender) =>
            {
                hybridWebView.Reload();
            });

            hybridWebView.RegisterBiometricAuthAvailableAction(data => DisplayAlert("Alert Xamarin", "BiometricAuthAvailableAction : " + data, "OK"));
            hybridWebView.RegisterBiometricAuthAction(data => Device.BeginInvokeOnMainThread(async () => await BiometricAuthentication()));
            hybridWebView.RegisterRememberUserAction(data => Device.BeginInvokeOnMainThread(async () => await RememberUser(data)));
            hybridWebView.RegisterLoginConfirmedAction(data => Device.BeginInvokeOnMainThread(async () =>await IngresoApp(data)));
        }

        private async Task RememberUser(string data)
        {
            bool isFingerprintAvailable = await CrossFingerprint.Current.IsAvailableAsync(false);
            if (!isFingerprintAvailable)
            {
                await DisplayAlert("Error",
                    "El biometrico no esta configurado.", "OK");
                return;
            }

            var result = await this.DisplayAlert("Alert!", "Quieres ingresar con biometria", "Si", "No");
            if (!result)
            {
                return;
            }

            AuthenticationRequestConfiguration conf =
                new AuthenticationRequestConfiguration("Autenticación",
                "Guardando el Usuario");

            var authResult = await CrossFingerprint.Current.AuthenticateAsync(conf);
            if (authResult.Authenticated)
            {
                var userRemember = JsonConvert.DeserializeObject<UserLogin>(data);
                Barrel.Current.Add(UserBiometric, userRemember, TimeSpan.FromDays(365));
                await DisplayAlert("Alert!", "Usuario Guardado correctamente", "OK");
                MessagingCenter.Send<LoginPageTest>(this, "SaveUser");

            }
            else
            {
                await DisplayAlert("Error", "Authentication failed", "OK");
            }
        }

        private async Task BiometricAuthentication()
        {
            bool isFingerprintAvailable = await CrossFingerprint.Current.IsAvailableAsync(false);
            if (!isFingerprintAvailable)
            {
                await DisplayAlert("Error",
                    "El biometrico no esta configurado.", "OK");
                return;
            }

            AuthenticationRequestConfiguration conf =
                new AuthenticationRequestConfiguration("Autenticación",
                "Autenticación");

            var authResult = await CrossFingerprint.Current.AuthenticateAsync(conf);
            if (authResult.Authenticated)
            {
                var user = Barrel.Current.Get<UserLogin>(UserBiometric);
                hybridWebView.UserLogin = user;
            }
            else
            {
                await DisplayAlert("Error", "Authentication failed", "OK");
            }
        }

        private async Task IngresoApp(string data)
        {
            //await DisplayAlert("Alert Xamarin", "LoginConfirmedAction : " + data, "OK");
            await Navigation.PushAsync(new MainPage());
        }

    }
}