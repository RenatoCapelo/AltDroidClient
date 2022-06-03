using System.Collections.Generic;
using RestSharp;
using System.Net.Http;
using Newtonsoft.Json;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using Altdroid.Adapters;
using Altdroid.Models;
using Altdroid.Logic.Models;
using Altdroid.Logic.Adapters;
using System;

namespace Altdroid.Logic.Fragments
{
    public class SignUpFragment : Fragment
    {
        public event EventHandler onLoginClick;
        public EditText editTextname;
        public EditText editTextEmail;
        public EditText editTextPassword;
        public EditText editTextBirthday;
        public RadioGroup gender;

        public Button signUp_btn;
        public Button login_btn;
        View view;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            view = inflater.Inflate(Resource.Layout.signupFragment, container, false);
            editTextname=view.FindViewById<EditText>(Resource.Id.signup_nameInput);
            editTextEmail = view.FindViewById<EditText>(Resource.Id.signup_emailInput);
            editTextPassword = view.FindViewById<EditText>(Resource.Id.signup_passwordInput);
            editTextBirthday = view.FindViewById<EditText>(Resource.Id.signup_dobInput);
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

        private async void SignUp_btn_Click(object sender, EventArgs e)
        {
            var idGender = int.Parse(view.FindViewById<RadioButton>(gender.CheckedRadioButtonId).Tag.ToString());
            
            using (var client = new RestClient("https://api.appstore.renatoventura.pt"))
            {
                var request = new RestRequest("user", Method.Post);
                request.AddBody(new
                {
                    email = editTextEmail.Text,
                    password = editTextPassword.Text,
                    name = editTextname.Text,
                    dob = DateTime.Parse(editTextBirthday.Text),
                    idGender
                }, "application/json");
                try
                {
                    var res = await client.ExecutePostAsync(request);
                    if (res.IsSuccessful)
                    {
                        onLoginClick.Invoke(sender, e);
                    }
                    else {
                        Toast.MakeText(this.Context, "Please confirm all fields and try again", ToastLength.Long).Show();
                    }
                    
                }
                catch (HttpRequestException ex)
                {
                    Toast.MakeText(this.Context, "There was an error... Try again", ToastLength.Long).Show();
                }
            }
        }
    }
}