using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using SpikeAuthentication.Dto;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpikeAuthentication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPageTest : ContentPage
    {
        public LoginPageTest()
        {
            InitializeComponent();

            hybridWebView.RegisterBiometricAuthAvailableAction(data => DisplayAlert("Alert Xamarin", "BiometricAuthAvailableAction : " + data, "OK"));
            hybridWebView.RegisterBiometricAuthAction(data => DisplayAlert("Alert Xamarin", "BiometricAuthAction : " + data, "OK"));
            hybridWebView.RegisterRememberUserAction(data => DisplayAlert("Alert Xamarin", "RememberUserAction : " + data, "OK"));
            hybridWebView.RegisterLoginConfirmedAction(data => IngresoApp(data));
        }

        private async void IngresoApp(string data)
        {
            await DisplayAlert("Alert Xamarin", "LoginConfirmedAction : " + data, "OK");
            Device.BeginInvokeOnMainThread(async () =>
                await Navigation.PushAsync(new MainPage())
            );
        }

        private async void Button_Clicked(object sender, EventArgs e)
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
                "Esta es una prueba de autenticación");

            var authResult = await CrossFingerprint.Current.AuthenticateAsync(conf);
            if (authResult.Authenticated)
            {
                var user = new UserLogin
                {
                    idNumber = "1073683899",
                    idType = "cc",
                    password = "123456789"
                };
                hybridWebView.UserLogin = user;
            }
            else
            {
                await DisplayAlert("Error", "Authentication failed", "OK");
            }

            
        }
    }
}