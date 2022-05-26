using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using App1.Models;
using Newtonsoft.Json;
using Square.Picasso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App1.Logic.Activities
{
    [Activity(Label = "AppDetails_Activity",ScreenOrientation =Android.Content.PM.ScreenOrientation.Portrait)]
    public class AppDetails_Activity : AppCompatActivity
    {
        AppToGet app;

        private TextView title;
        private TextView price;
        private ImageView logo;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            app = JsonConvert.DeserializeObject<AppToGet>(Intent.GetStringExtra("app"));
            SetContentView(Resource.Layout.AppDetails);
            title = FindViewById<TextView>(Resource.Id.appDetails_Title);
            price = FindViewById<TextView>(Resource.Id.appDetails_Price);
            logo = FindViewById<ImageView>(Resource.Id.appDetails_logo);

            title.Text = app.title;
            price.Text = "Gratuito";
            logo.SetImageResource(Resource.Drawable.noImage);
            if(app.Icon.HasValue)
            {
                var url = $"https://api.appstore.renatoventura.pt/storage/apps/{app.developer.devGuid}/{app.applicationGuid}/photos/{app.Icon}.png";
                Picasso
                    .Get()
                    .Load(url)
                    .Error(Resource.Drawable.trypsterLogo)
                    .Into(logo);
            }
            // Create your application here
        }
    }
}