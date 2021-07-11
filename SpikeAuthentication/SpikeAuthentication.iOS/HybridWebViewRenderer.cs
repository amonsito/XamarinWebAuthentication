using Foundation;
using SpikeAuthentication;
using SpikeAuthentication.Dto;
using SpikeAuthentication.iOS;
using System.ComponentModel;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace SpikeAuthentication.iOS
{
    public class HybridWebViewRenderer : WkWebViewRenderer, IWKScriptMessageHandler
    {
        const string JavaScriptFunction = "var HostApp = { loginConfirmed: function (data) { window.webkit.messageHandlers.LoginConfirmed.postMessage(data); }, RememberUser: function (data) { window.webkit.messageHandlers.RememberUser.postMessage(data); }, BiometricAuth: function (data) { window.webkit.messageHandlers.BiometricAuth.postMessage(data); }, BiometricAuthAvailable: function (data) { window.webkit.messageHandlers.BiometricAuthAvailable.postMessage(data); } }";
        WKUserContentController userController;

        public HybridWebViewRenderer() : this(new WKWebViewConfiguration())
        {
        }

        public HybridWebViewRenderer(WKWebViewConfiguration config) : base(config)
        {
            userController = config.UserContentController;
            var script = new WKUserScript(new NSString(JavaScriptFunction), WKUserScriptInjectionTime.AtDocumentEnd, false);
            userController.AddUserScript(script);
            userController.AddScriptMessageHandler(this, "LoginConfirmed");

            userController.AddScriptMessageHandler(this, "RememberUser");

            userController.AddScriptMessageHandler(this, "BiometricAuth");

            userController.AddScriptMessageHandler(this, "BiometricAuthAvailable");
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                userController.RemoveAllUserScripts();
                userController.RemoveScriptMessageHandler("LoginConfirmed");
                userController.RemoveScriptMessageHandler("RememberUser");
                userController.RemoveScriptMessageHandler("BiometricAuth");
                userController.RemoveScriptMessageHandler("BiometricAuthAvailable");
                HybridWebView hybridWebView = e.OldElement as HybridWebView;
                hybridWebView.Cleanup();
            }

            if (e.NewElement != null)
            {
                string filename = $"{((HybridWebView)Element).Uri}";
                LoadRequest(new NSUrlRequest(new NSUrl(filename)));
                Element.PropertyChanged += OnElementPropertyChanged;
            }
        }

        private void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName == nameof(UserLogin))
            {
                var customWebView = Element as HybridWebView;
                LoginUser(customWebView.UserLogin);
            }
        }

        public void LoginUser(UserLogin user)
        {
            string scriptUser = "enterCredentials(\"" + user.idNumber + "\"" +
                           ",\"" + user.idType + "\",\"" + user.password + "\");";
            (Element as WebView).EvaluateJavaScriptAsync(scriptUser);
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            if (message.Name == "LoginConfirmed")
            {
                ((HybridWebView)Element).LoginConfirmed(message.Body.ToString());
            }
            else if (message.Name == "RememberUser")
            {
                ((HybridWebView)Element).RememberUser(message.Body.ToString());
            }
            else if (message.Name == "BiometricAuth")
            {
                ((HybridWebView)Element).BiometricAuth(message.Body.ToString());
            }
            else if (message.Name == "BiometricAuthAvailable")
            {
                ((HybridWebView)Element).BiometricAuthAvailable(message.Body.ToString());
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

    }
}