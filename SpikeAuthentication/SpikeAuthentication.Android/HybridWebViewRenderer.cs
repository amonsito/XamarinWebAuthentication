using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net.Http;
using Android.Webkit;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using SpikeAuthentication;
using SpikeAuthentication.Droid;
using SpikeAuthentication.Dto;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace SpikeAuthentication.Droid
{

    public class HybridWebViewRenderer : WebViewRenderer
    {
        //const string JavascriptFunction = "var HostApp = { loginConfirmed: function(data) { jsBridge.LoginConfirmed(data); },RememberUser: function(data) { jsBridge.RememberUser(data); },BiometricAuth: function(data) { jsBridge.BiometricAuth(data); },BiometricAuthAvailable: function() { console.log('test');jsBridge.BiometricAuthAvailable(); }}";
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

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                Control.RemoveJavascriptInterface("HostApp");
                ((HybridWebView)Element).Cleanup();
            }
            if (e.NewElement != null)
            {
                var webSettings = Control.Settings;
                webSettings.JavaScriptEnabled = true;
                webSettings.AllowFileAccess = true;
                webSettings.AllowContentAccess = true;
                webSettings.AllowFileAccessFromFileURLs = true;
                webSettings.AllowUniversalAccessFromFileURLs = true;
                webSettings.BlockNetworkLoads = false;
                webSettings.JavaScriptCanOpenWindowsAutomatically = true;
                webSettings.SetSupportMultipleWindows(false);
                webSettings.DomStorageEnabled = true;
                webSettings.DatabaseEnabled = true;

                Control.AddJavascriptInterface(new JSBridge(this), "HostApp");

#if DEBUG //this is for ssl error
                Control.SetWebViewClient(new InvalidWebViewClient());
#endif
                Control.LoadUrl(((HybridWebView)Element).Uri);

                /*Test Camera*/
                //var thisActivity = Forms.Context as Activity;
                //var a = ContextCompat.CheckSelfPermission(thisActivity, Manifest.Permission.Camera);
                //if (a != Permission.Granted)
                //{
                //    ActivityCompat.RequestPermissions(thisActivity, new String[] { Manifest.Permission.Camera }, 20);
                //}
                //Control.SetWebChromeClient(new MyWebChromeClient(thisActivity));
                //Control.Settings.JavaScriptEnabled = true;

            }
        }

        //protected override FormsWebChromeClient GetFormsWebChromeClient()
        //{
        //    return new CameraFormsWebChromeClient();
        //}

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

    /// <summary>
    /// Only for ssl error
    /// </summary>
    public class InvalidWebViewClient : WebViewClient
    {
        public override void OnReceivedSslError(Android.Webkit.WebView view, SslErrorHandler handler, SslError error)
        {
            handler.Proceed();
        }
    }
   
}