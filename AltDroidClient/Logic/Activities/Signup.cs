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

namespace App1.Activities
{
    [Activity(Label = "Signup")]
    public class Signup : Activity
    {
        TextView textView1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            string userToken = Intent.GetStringExtra("user") ?? "Data not available";
            // Create your application here
            SetContentView(Resource.Layout.signup);

            textView1 = FindViewById<TextView>(Resource.Id.textView1);
            textView1.Text = userToken;
            
        }
    }
}