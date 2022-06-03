using System;
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
using Android.Content;
using Altdroid.Logic.Activities;
using Altdroid.Logic.Views;
using Xamarin.Essentials;

namespace Altdroid.Logic.Fragments
{
    
    public class SettingsDialogFragment:DialogFragment
    {
        public event EventHandler onSignoutClick;
        private Button btnSignOut;
        private TextView textView;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
            View view = inflater.Inflate(Resource.Layout.settingsDialog_Fragment, container,false);
            var res = SecureStorage.GetAsync("user");
            res.Wait();
            var user = JsonConvert.DeserializeObject<User>(res.Result);
            btnSignOut = view.FindViewById<Button>(Resource.Id.settings_BtnSignOut);
            textView = view.FindViewById<TextView>(Resource.Id.settings_TextView);
            textView.Text = "Currently logged in as " + user.name;
            btnSignOut.Click += (sender, args) => onSignoutClick.Invoke(sender, args);
            return view;
        }

       
    }
}