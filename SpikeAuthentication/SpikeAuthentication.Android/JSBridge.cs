using Android.Webkit;
using Java.Interop;
using System;

namespace SpikeAuthentication.Droid
{
    public class JSBridge : Java.Lang.Object
    {
        readonly WeakReference<HybridWebViewRenderer> hybridWebViewRenderer;

        public JSBridge(HybridWebViewRenderer hybridRenderer)
        {
            hybridWebViewRenderer = new WeakReference<HybridWebViewRenderer>(hybridRenderer);
        }

        [JavascriptInterface]
        [Export("biometricAuthAvailable")]
        public bool BiometricAuthAvailable()
        {
            HybridWebViewRenderer hybridRenderer;

            if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out hybridRenderer))
            {
                return ((HybridWebView)hybridRenderer.Element).BiometricAuthAvailable();
            }
            return false;
        }

        [JavascriptInterface]
        [Export("biometricAuth")]
        public void biometricAuth()
        {
            HybridWebViewRenderer hybridRenderer;

            if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out hybridRenderer))
            {
                ((HybridWebView)hybridRenderer.Element).BiometricAuth();
            }
        }

        [JavascriptInterface]
        [Export("rememberUser")]
        public void RegisterRememberUser(string idNumber,string idType, string password)
        {
            HybridWebViewRenderer hybridRenderer;

            if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out hybridRenderer))
            {
                ((HybridWebView)hybridRenderer.Element).RememberUser(idNumber, idType, password);
            }
        }

        [JavascriptInterface]
        [Export("loginConfirmed")]
        public void LoginConfirmed(string data)
        {
            HybridWebViewRenderer hybridRenderer;

            if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out hybridRenderer))
            {
                ((HybridWebView)hybridRenderer.Element).LoginConfirmed(data);
            }
        }
    }
}