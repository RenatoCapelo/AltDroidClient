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
using App1.Models;
using AndroidX.AppCompat.App;
using App1.Logic.Fragments;

namespace App1.Activities
{
    [Activity(Label = "Login",Theme = "@style/AppTheme",ScreenOrientation =Android.Content.PM.ScreenOrientation.Portrait)]
    public class Login : AppCompatActivity
    {
        private LoginFragment _loginFragment;
        private SignUpFragment _signUpFragment;

        private Fragment currentFragment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.Login);
                        
            _loginFragment = new LoginFragment();
            _loginFragment.onSignUpClick += _loginFragment_onSignUpClick;
            _loginFragment.onLoginSuccess += _loginFragment_onLoginSuccess;
            _signUpFragment = new SignUpFragment();
            _signUpFragment.onLoginClick += _signUpFragment_onLoginClick;
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.login_FragmentContainer, _signUpFragment, "SignUpFragment");
            trans.Hide(_signUpFragment);
            trans.Add(Resource.Id.login_FragmentContainer, _loginFragment, "LoginFragment");
            trans.Commit();
            currentFragment = _loginFragment;

            // Create your application here
        }

        private void _loginFragment_onLoginSuccess(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(MainActivity));
            this.StartActivity(intent);
            this.Finish();
        }

        private void _signUpFragment_onLoginClick(object sender, EventArgs e)
        {
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Hide(currentFragment);
            trans.Show(_loginFragment);
            trans.AddToBackStack(null);
            currentFragment = _loginFragment;
            trans.Commit();
        }

        private void _loginFragment_onSignUpClick(object sender, EventArgs e)
        {
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Hide(currentFragment);
            trans.Show(_signUpFragment);
            trans.AddToBackStack(null);
            currentFragment = _signUpFragment;
            trans.Commit();
        }
    }
}