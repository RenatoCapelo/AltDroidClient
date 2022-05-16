using System.Collections.Generic;
using RestSharp;
using System.Net.Http;
using Newtonsoft.Json;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using App1.Adapters;
using App1.Models;
using App1.Logic.Models;
using App1.Logic.Adapters;
using System;

namespace App1.Logic.Fragments
{
    public class SignUpFragment : Fragment
    {
        public event EventHandler onLoginClick;
        public EditText name;
        public EditText email;
        public EditText password;
        public EditText birthday;
        public RadioGroup gender;

        public Button signUp_btn;
        public Button login_btn;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.signupFragment, container, false);
            name=view.FindViewById<EditText>(Resource.Id.signup_nameInput);
            email = view.FindViewById<EditText>(Resource.Id.signup_emailInput);
            password = view.FindViewById<EditText>(Resource.Id.signup_passwordInput);
            birthday = view.FindViewById<EditText>(Resource.Id.signup_dobInput);
            gender = view.FindViewById<RadioGroup>(Resource.Id.signup_Gender);
            signUp_btn = view.FindViewById<Button>(Resource.Id.signup_btnSignup);
            login_btn = view.FindViewById<Button>(Resource.Id.signup_btnLogin);

            signUp_btn.Click += SignUp_btn_Click;
            login_btn.Click += Login_btn_Click;

            return view;
        }

        private void Login_btn_Click(object sender, EventArgs e)
        {
            onLoginClick.Invoke(sender, e);
        }

        private void SignUp_btn_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}