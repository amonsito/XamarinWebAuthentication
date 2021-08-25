using Foundation;
using SpikeAuthentication;
using SpikeAuthentication.Dto;
using SpikeAuthentication.iOS;
using System;
using System.ComponentModel;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace SpikeAuthentication.iOS
{
    public class HybridWebViewRenderer : WkWebViewRenderer, IWKScriptMessageHandler
    {
        const string JavaScriptFunction = "var HostApp = { loginConfirmed: function (data) { window.webkit.messageHandlers.LoginConfirmed.postMessage(data); }, rememberUser: function (idNumber,idType,password) { window.webkit.messageHandlers.RememberUser.postMessage({IdNumber:idNumber,IdType:idType,Password:password}); }, biometricAuth: function (data) { window.webkit.messageHandlers.BiometricAuth.postMessage(data); }, biometricAuthAvailable: function (data) { return window.webkit.messageHandlers.BiometricAuthAvailable.postMessage(data);} };";
        WKUserContentController userController;

        public HybridWebViewRenderer() : this(new WKWebViewConfiguration())
        {
#if DEBUG
            //only for ssl-fail
            NavigationDelegate = new CWKNavigationDelegate();
#endif

        }

        public HybridWebViewRenderer(WKWebViewConfiguration config) : base(config)
        {
            config.Preferences.JavaScriptEnabled = true;
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
                LoadRequest(new NSUrlRequest(new NSUrl((Element as HybridWebView).Uri)));
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
                var values = message.Body;
                var idNumber = values.ValueForKey(new NSString("IdNumber")).ToString();
                var idType = values.ValueForKey(new NSString("IdType")).ToString();
                var password = values.ValueForKey(new NSString("Password")).ToString();

                ((HybridWebView)Element).RememberUser(idNumber, idType, password);
            }
            else if (message.Name == "BiometricAuth")
            {
                ((HybridWebView)Element).BiometricAuth();
            }
            else if (message.Name == "BiometricAuthAvailable")
            {
                string scriptAvaliableBiometric = ((HybridWebView)Element).BiometricAuthAvailable() ? "$(\"#btnBiometric\").show();" : "$(\"#btnBiometric\").hide();";
                message.WebView.EvaluateJavaScript(scriptAvaliableBiometric, null);
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

    /// <summary>
    /// Only for ssl-fail
    /// </summary>
    public class CWKNavigationDelegate : WKUserContentController, IWKNavigationDelegate, INSUrlConnectionDataDelegate
    {
        [Export("webView:didReceiveAuthenticationChallenge:completionHandler:")]
        public virtual void DidReceiveAuthenticationChallenge(WKWebView webView, NSUrlAuthenticationChallenge nac, Action<NSUrlSessionAuthChallengeDisposition, NSUrlCredential> NC)
        {
            if (nac.ProtectionSpace.AuthenticationMethod.Equals("NSURLAuthenticationMethodServerTrust"))
            {
                nac.Sender.UseCredential(new NSUrlCredential(nac.ProtectionSpace.ServerSecTrust), nac);
                nac.Sender.ContinueWithoutCredential(nac);
            }

        }
    }

}