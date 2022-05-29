using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using Fragment = AndroidX.Fragment.App.Fragment;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using RestSharp;
using Google.Android.Material.Snackbar;
using Altdroid.Models;
using AndroidX.AppCompat.App;
using Altdroid.Logic.Fragments;
using Newtonsoft.Json;

namespace Altdroid.Logic.Fragments
{
    public class LoginFragment : Fragment
    {
        Button btnLogin;
        Button btnSignup;
        EditText inputLogin;
        EditText passwordInput;

        public event EventHandler onSignUpClick;
        public event EventHandler onLoginSuccess;
        public override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.loginFragment, container, false);
            btnLogin = view.FindViewById<Button>(Resource.Id.btnLogin);
            btnSignup = view.FindViewById<Button>(Resource.Id.btnSignup);
            inputLogin = view.FindViewById<EditText>(Resource.Id.login_emailInput);
            passwordInput = view.FindViewById<EditText>(Resource.Id.login_passwordInput);


            btnLogin.Click += BtnLogin_Click;
            btnSignup.Click += BtnSignup_Click;
            return view;
            
        }

        private async void BtnLogin_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(inputLogin.Text) || string.IsNullOrEmpty(passwordInput.Text))
            {
                Toast.MakeText(this.Context, "You Must fill all fields!", ToastLength.Long).Show();
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
                    try
                    {
                        var response = await httpClient.PostAsync<AuthenticationResponse>(request);
                        await SecureStorage.SetAsync("token", response.token);
                        await SecureStorage.SetAsync("user",JsonConvert.SerializeObject(response.user));
                        onLoginSuccess.Invoke(this, e);
                    }
                    catch (Exception ex)
                    {
                        Toast.MakeText(this.Context,"There was an error... Try again",ToastLength.Long).Show();
                    }
                }
            }
        }
        private async void BtnSignup_Click(object sender, System.EventArgs e)
        {
            onSignUpClick.Invoke(this, e);
        }

    }
}