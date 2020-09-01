using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Auth;

namespace SocialNetwork.Android
{
    [Activity(Label = "SocialNetwork.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var authenticatior = new OAuth2Authenticator(
                "socialnetwork_implicit", "read",
                new Uri("http://192.168.0.3:22710/connect/authorize"),
                new Uri("http://localhost:28037")
            );

            authenticatior.Completed += (sender, args) =>
            {
                
            };

            StartActivity(authenticatior.GetUI(this));
        }
    }
}

