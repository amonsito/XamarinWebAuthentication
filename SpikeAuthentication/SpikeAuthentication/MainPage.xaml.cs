using MonkeyCache.FileStore;
using Newtonsoft.Json;
using SpikeAuthentication.Dto;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SpikeAuthentication
{
    public partial class MainPage : ContentPage
    {
        const string UserBiometric = "UserBiometric";
        public MainPage()
        {
            InitializeComponent();
            InitUserData();

            MessagingCenter.Subscribe<LoginPageTest>(this, "SaveUser", (sender) =>
            {
                InitUserData();
            });
        }

        private void InitUserData()
        {
            if (Barrel.Current.Exists(UserBiometric))
            {
                var userRemember = Barrel.Current.Get<UserLogin>(UserBiometric);
                NumberUser.Text = $"Guardado: {userRemember.idNumber}";
                this.botonOlvidarUsuario.IsVisible = true;
                NumberUser.IsVisible = true;
            }
            else
            {
                this.botonOlvidarUsuario.IsVisible = false;
                NumberUser.IsVisible = false;
            }
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            Barrel.Current.Empty(UserBiometric);
            InitUserData();
        }

        private void Button_Clicked2(object sender, System.EventArgs e)
        {
            MessagingCenter.Send<MainPage>(this, "Hi");
            MessagingCenter.Unsubscribe<LoginPageTest>(this, "SaveUser");
            Navigation.PopAsync();
        }
    }
}
