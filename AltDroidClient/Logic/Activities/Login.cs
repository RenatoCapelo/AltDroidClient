using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using RestSharp;
using Google.Android.Material.Snackbar;
using App1.Models;

namespace App1.Activities
{
    [Activity(Label = "Login")]
    public class Login : Activity
    {
        Button btnLogin;
        Button btnSignup;
        EditText inputLogin;
        EditText passwordInput;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);
            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            btnSignup = FindViewById<Button>(Resource.Id.btnSignup);
            inputLogin = FindViewById<EditText>(Resource.Id.login_emailInput);
            passwordInput = FindViewById<EditText>(Resource.Id.login_passwordInput);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            btnLogin.Click += BtnLogin_Click;
            btnSignup.Click += BtnSignup_Click;

            // Create your application here
        }

        private async void BtnLogin_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(inputLogin.Text) || string.IsNullOrEmpty(passwordInput.Text))
            {
                Toast.MakeText(this, "You Must fill all fields!", ToastLength.Long);
            }
            else
            {
                using (RestClient httpClient = new RestClient(baseUrl: "https://api.appstore.renatoventura.pt"))
                {
                    RestRequest request = new RestRequest("Authentication", Method.Post);
                    request.AddBody(new
                    {
                        email = inputLogin.Text,
                        password = passwordInput.Text
                    }, "application/json");

                    var response = await httpClient.PostAsync<AuthenticationResponse>(request);
                    await SecureStorage.SetAsync("token", response.token);
                }
            }
        }
        private async void BtnSignup_Click(object sender, System.EventArgs e)
        {
            //Toast.MakeText(this.ApplicationContext, $"AYO! {await SecureStorage.GetAsync("token")}",ToastLength.Long).Show();
            Intent signupPage = new Intent(this, typeof(Signup));
            signupPage.PutExtra("user", await SecureStorage.GetAsync("token"));
            StartActivity(signupPage);
        }
    }
}