using Android.Animation;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Altdroid.Activities;
using Java.Interop;
using System;
using Altdroid.Logic.Fragments;
using Xamarin.Essentials;
using static Android.Animation.ValueAnimator;

using Fragment = AndroidX.Fragment.App.Fragment;
using System.IdentityModel.Tokens.Jwt;
using Altdroid.Logic.Activities;
using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;

namespace Altdroid
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
        private Button settings;

        private Fragment currentFragment;
        private AppsFragment _appsFragment;
        private GamesFragment _gamesFragment;
        private SearchFragment _searchFragment;
        private LinearLayout _llCategories;

        private EditText _searchTextBox;
        

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjUwMjEzQDMyMzAyZTMxMmUzMGJsWGNYekpZU3RNdjY5Q1IyQ1ZqZU9tSHBMSTl1cmszc0NXcldLejhMOG89");
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
            settings = FindViewById<Button>(Resource.Id.main_btnProfile);
            _llCategories = FindViewById<LinearLayout>(Resource.Id.main_linearLayout_categories);
            _appsFragment = new AppsFragment();
            _gamesFragment = new GamesFragment();
            _searchFragment = new SearchFragment();
            
            _searchTextBox = FindViewById<EditText>(Resource.Id.main_searchTextInput);
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.main_fragmentContainer, _searchFragment, "SearchFragment");
            trans.Hide(_searchFragment);
            trans.Add(Resource.Id.main_fragmentContainer, _gamesFragment, "GamesFragment");
            trans.Hide(_gamesFragment);
            trans.Add(Resource.Id.main_fragmentContainer, _appsFragment, "AppsFragment");
            trans.Commit();
            currentFragment = _appsFragment;
            btnApps.Click += BtnApps_Click;
            btnGames.Click += BtnGames_Click;
            _searchTextBox.TextChanged += _searchTextBox_TextChanged;
            settings.Click += SettingsOnClick;

            //TODO - Refactor to user object
            string.IsNullOrWhiteSpace(await SecureStorage.GetAsync("token"));

        }

        private void SettingsOnClick(object sender, EventArgs e)
        {
            var trans = SupportFragmentManager.BeginTransaction();
            SettingsDialogFragment dialog = new SettingsDialogFragment();
            dialog.Show(trans, "Settings");
            dialog.onSignoutClick += Dialog_onSignoutClick;
        }

        private async void Dialog_onSignoutClick(object sender, EventArgs e)
        {
            SecureStorage.Remove("user");
            SecureStorage.Remove("token");
            var intent = new Intent(this, typeof(Login));
            this.StartActivity(intent);
            this.Finish();
        }

        private void _searchTextBox_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(_searchTextBox.Text))
            {
                ShowFragment(_appsFragment);
            }
            else
            {
                ShowFragment(_searchFragment);
                _searchFragment.newQuery(_searchTextBox.Text);
            }
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
            if(fragment == _gamesFragment)
            {
                /*trans.SetCustomAnimations(Resource.Animation.slide_apps_to_games,
                    Resource.Animation.slide_games_to_apps);*/
                changeColors(false);
            }
            if(fragment == _searchFragment)
            {
                _llCategories.Visibility = Android.Views.ViewStates.Gone;
            }
            if(fragment == _appsFragment || fragment == _gamesFragment)
            {
                trans.SetCustomAnimations(Resource.Animation.slide_games_to_apps,Resource.Animation.slide_apps_to_games);
                _llCategories.Visibility = Android.Views.ViewStates.Visible;
            }
            trans.Hide(currentFragment);
            trans.Show(fragment);
            trans.AddToBackStack(null);
            trans.Commit();

            SupportFragmentManager.ExecutePendingTransactions();

            currentFragment = fragment;
            return;

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