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
using System.IdentityModel.Tokens.Jwt;

namespace App1
{
    [Activity(Label = "@string/app_name",
        Theme = "@style/Theme.AppCompat.DayNight.NoActionBar",
        MainLauncher = true,
        WindowSoftInputMode = Android.Views.SoftInput.AdjustNothing,
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
        
    public class MainActivity : AppCompatActivity
        //, IAnimatorUpdateListener
    {

        Button btnApps;
        Button btnGames;
        LinearLayout mainContainer;

        private Fragment currentFragment;
        private AppsFragment _appsFragment;
        private GamesFragment _gamesFragment;

        private EditText _searchTextBox;
        

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            //await SecureStorage.SetAsync("token", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiJ9.eyJleHAiOjE2NTAyMjAwMzUuMCwiR3VpZCI6ImFjNDUyM2EyLTJhMzMtNDMyOS1hMWE1LTA4MjRkMDhkNDk5MSIsIk5hbWUiOiJSZW5hdG8iLCJyb2xlIjoiRGV2In0.kbRZFlBZLBi2Qd1fMh5U9Mrpox0PTvWZ7da6BgnP4ihtqReEwNHw8_vowWE9GG72WfcfRULxcmDp7iNpUE6LNQ");
            var handler = new JwtSecurityTokenHandler();
            bool tokenValid = false;
            if( handler.CanReadToken(await SecureStorage.GetAsync("token")))
            {
                var token = handler.ReadJwtToken(await SecureStorage.GetAsync("token"));
                var exp = token.ValidTo;
                if (exp > DateTime.Now)
                    tokenValid = true;
            }
            if (!tokenValid)
            {
                var intent = new Intent(this, typeof(Login));
                this.StartActivity(intent);
                this.Finish();
            }
            

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            btnApps = FindViewById<Button>(Resource.Id.main_btnApps);
            btnGames = FindViewById<Button>(Resource.Id.main_btnGames);
            mainContainer = FindViewById<LinearLayout>(Resource.Id.main_MainContainer);
            _searchTextBox = FindViewById<EditText>(Resource.Id.main_searchTextInput);
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
            _searchTextBox.Click += _searchTextBox_Click;

            //TODO - Refactor to user object
            string.IsNullOrWhiteSpace(await SecureStorage.GetAsync("token"));

        }

        private void _searchTextBox_Click(object sender, EventArgs e)
        {
            
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