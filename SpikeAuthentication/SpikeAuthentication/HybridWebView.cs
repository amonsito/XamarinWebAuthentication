using SpikeAuthentication.Dto;
using System;
using Xamarin.Forms;

namespace SpikeAuthentication
{
    public class HybridWebView : WebView
    {
        public static readonly BindableProperty userLoginProperty = BindableProperty.Create(propertyName: "UserLogin",
              returnType: typeof(UserLogin),
              declaringType: typeof(HybridWebView),
              defaultValue: default(UserLogin));

        public UserLogin UserLogin
        {
            get { return (UserLogin)GetValue(userLoginProperty); }
            set { SetValue(userLoginProperty, value); }
        }

        /// <summary>
        /// Action for biometric authentication Available action
        /// </summary>
        Action<string> biometricAuthAvailableAction;
        public void RegisterBiometricAuthAvailableAction(Action<string> callback) => biometricAuthAvailableAction = callback;

        /// <summary>
        /// Action for biometric authentication action
        /// </summary>
        Action<string> biometricAuthAction;
        public void RegisterBiometricAuthAction(Action<string> callback) => biometricAuthAction = callback;

        /// <summary>
        /// Action for remember user action
        /// </summary>
        Action<string> rememberUserAction;
        public void RegisterRememberUserAction(Action<string> callback) => rememberUserAction = callback;

        /// <summary>
        /// Action for login confirmed action
        /// </summary>
        Action<string> loginConfirmedAction;
        public void RegisterLoginConfirmedAction(Action<string> callback) => loginConfirmedAction = callback;


        public static readonly BindableProperty UriProperty = BindableProperty.Create(
            propertyName: "Uri",
            returnType: typeof(string),
            declaringType: typeof(HybridWebView),
            defaultValue: default(string));


        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        public void Cleanup()
        {
            biometricAuthAvailableAction = null;
            biometricAuthAction = null;
            rememberUserAction = null;
            loginConfirmedAction = null;
        }

        public void BiometricAuthAvailable(string data)
        {
            if (biometricAuthAvailableAction == null || data == null)
            {
                return;
            }
            biometricAuthAvailableAction.Invoke(data);
        }

        public void BiometricAuth(string data)
        {
            if (biometricAuthAction == null || data == null)
            {
                return;
            }
            biometricAuthAction.Invoke(data);
        }

        public void RememberUser(string data)
        {
            if (rememberUserAction == null || data == null)
            {
                return;
            }
            rememberUserAction.Invoke(data);
        }

        public void LoginConfirmed(string data)
        {
            if (loginConfirmedAction == null || data == null)
            {
                return;
            }
            loginConfirmedAction.Invoke(data);
        }

    }
}
