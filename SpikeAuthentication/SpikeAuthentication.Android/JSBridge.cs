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
        [Export("BiometricAuthAvailable")]
        public void BiometricAuthAvailable(string data)
        {
            HybridWebViewRenderer hybridRenderer;

            if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out hybridRenderer))
            {
                ((HybridWebView)hybridRenderer.Element).BiometricAuthAvailable(data);
            }
        }

        [JavascriptInterface]
        [Export("BiometricAuth")]
        public void BiometricAuth(string data)
        {
            HybridWebViewRenderer hybridRenderer;

            if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out hybridRenderer))
            {
                ((HybridWebView)hybridRenderer.Element).BiometricAuth(data);
            }
        }

        [JavascriptInterface]
        [Export("RememberUser")]
        public void RegisterRememberUser(string data)
        {
            HybridWebViewRenderer hybridRenderer;

            if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out hybridRenderer))
            {
                ((HybridWebView)hybridRenderer.Element).RememberUser(data);
            }
        }

        [JavascriptInterface]
        [Export("LoginConfirmed")]
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