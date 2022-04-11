using Android.Animation;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using App1.Activities;
using Java.Interop;
using System;
using Xamarin.Essentials;
using static Android.Animation.ValueAnimator;

namespace App1
{
    [Activity(Label = "@string/app_name",
        Theme = "@style/Theme.AppCompat.DayNight.NoActionBar"
        ,MainLauncher = true
        )]
    public class MainActivity : AppCompatActivity
        //, IAnimatorUpdateListener
    {

        Button btnApps;
        Button btnGames;
        LinearLayout mainContainer;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            btnApps = FindViewById<Button>(Resource.Id.main_btnApps);
            btnGames = FindViewById<Button>(Resource.Id.main_btnGames);
            mainContainer = FindViewById<LinearLayout>(Resource.Id.main_MainContainer);

            btnApps.Click += BtnApps_Click;
            btnGames.Click += BtnGames_Click;

            //TODO - Refactor to user object
            string.IsNullOrWhiteSpace(await SecureStorage.GetAsync("token"));

        }

        private void BtnGames_Click(object sender, System.EventArgs e)
        {
            changeColors(false);
        }

        private void BtnApps_Click(object sender, System.EventArgs e)
        {
            changeColors();
        }

        private void changeColors(bool fromGame = true)
        {
            var time = 250;
            TransitionDrawable transition = (TransitionDrawable)mainContainer.Background;

            if (fromGame)
            {
                transition.ReverseTransition(time);
            }
            else
            {
                transition.StartTransition(time);
            }
            /*
            int colorFrom = fromGame ? Resource.Color.gamesBg : Resource.Color.appsBg;
            int colorTo = fromGame ? Resource.Color.appsBg : Resource.Color.gamesBg;

            ValueAnimator colorAnimator = ValueAnimator.OfObject(new ArgbEvaluator(), colorFrom, colorTo);
            colorAnimator.SetDuration(250);
            colorAnimator.AddUpdateListener(this);
            colorAnimator.Start();*/
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        /*
        public void OnAnimationUpdate(ValueAnimator animation)
        {
            int color = (int)animation.AnimatedValue;
            
            mainContainer.SetBackgroundColor();
        }
        */
    }

}