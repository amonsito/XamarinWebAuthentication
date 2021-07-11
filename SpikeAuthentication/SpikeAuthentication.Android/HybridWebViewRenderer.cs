using Android.Content;
using SpikeAuthentication;
using SpikeAuthentication.Droid;
using SpikeAuthentication.Dto;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace SpikeAuthentication.Droid
{

     public class HybridWebViewRenderer : WebViewRenderer
    {
        const string JavascriptFunction = "var HostApp = { loginConfirmed: function(data) { jsBridge.LoginConfirmed(data); },RememberUser: function(data) { jsBridge.RememberUser(data); },BiometricAuth: function(data) { jsBridge.BiometricAuth(data); },BiometricAuthAvailable: function(data) { jsBridge.BiometricAuthAvailable(data); }}";
        Context _context;

        public HybridWebViewRenderer(Context context) : base(context)
        {
            _context = context;
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(UserLogin))
            {
                var customWebView = Element as HybridWebView;
                LoginUser(customWebView.UserLogin);
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                Control.RemoveJavascriptInterface("jsBridge");
                ((HybridWebView)Element).Cleanup();
            }
            if (e.NewElement != null)
            {
                Control.SetWebViewClient(new JavascriptWebViewClient(this, $"javascript: {JavascriptFunction}"));
                Control.AddJavascriptInterface(new JSBridge(this), "jsBridge");
                Control.LoadUrl($"{((HybridWebView)Element).Uri}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ((HybridWebView)Element).Cleanup();
            }
            base.Dispose(disposing);
        }

        public void LoginUser(UserLogin user)
        {
            string script = "enterCredentials(\"" + user.idNumber + "\"" +
                           ",\"" + user.idType + "\",\"" + user.password + "\");";
            Control.EvaluateJavascript(script, null);
        }
    }
}