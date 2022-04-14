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
using App1.Logic.Fragments;
using Xamarin.Essentials;
using static Android.Animation.ValueAnimator;
using Fragment = AndroidX.Fragment.App.Fragment;

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

        private Fragment currentFragment;
        private AppsFragment _appsFragment;
        private GamesFragment _gamesFragment;
        

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            btnApps = FindViewById<Button>(Resource.Id.main_btnApps);
            btnGames = FindViewById<Button>(Resource.Id.main_btnGames);
            mainContainer = FindViewById<LinearLayout>(Resource.Id.main_MainContainer);
            _appsFragment = new AppsFragment();
            _gamesFragment = new GamesFragment();
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.main_fragmentContainer, _gamesFragment, "GamesFragment");
            trans.Hide(_gamesFragment);
            trans.Add(Resource.Id.main_fragmentContainer, _appsFragment, "AppsFragment");
            trans.Commit();
            currentFragment = _appsFragment;
            btnApps.Click += BtnApps_Click;
            btnGames.Click += BtnGames_Click;

            //TODO - Refactor to user object
            string.IsNullOrWhiteSpace(await SecureStorage.GetAsync("token"));

        }

        private void BtnGames_Click(object sender, System.EventArgs e)
        {
            //changeColors(false);
            ShowFragment(_gamesFragment);
        }

        private void BtnApps_Click(object sender, System.EventArgs e)
        {
            //changeColors();
            ShowFragment(_appsFragment);
        }

        private void ShowFragment(Fragment fragment)
        {
            if (fragment == currentFragment)
                return;
            
            var trans = SupportFragmentManager.BeginTransaction();
            if (fragment == _appsFragment)
            {
                /*trans.SetCustomAnimations(Resource.Animation.slide_games_to_apps,
                    Resource.Animation.slide_apps_to_games);*/
                changeColors(true);
            }
            else
            {
                /*trans.SetCustomAnimations(Resource.Animation.slide_apps_to_games,
                    Resource.Animation.slide_games_to_apps);*/
                changeColors(false);
            }
            trans.SetCustomAnimations(Resource.Animation.slide_games_to_apps,
                Resource.Animation.slide_apps_to_games);
            trans.Hide(currentFragment);
            trans.Show(fragment);
            trans.AddToBackStack(null);
            trans.Commit();

            currentFragment = fragment;

        }

        private void changeColors(bool fromGame = true)
        {
            var time = 250;
            TransitionDrawable transition = (TransitionDrawable)mainContainer.Background!;

            if (fromGame)
            {
                transition.ReverseTransition(time);
            }
            else
            {
                transition.StartTransition(time);
            }
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