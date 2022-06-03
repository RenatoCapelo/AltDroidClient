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
using System;

namespace Altdroid.Logic.Fragments
{

    public class SearchFragment : Fragment
    {
        private List<AppToGet> searchAppsList;
        private RecyclerView rv_searchApps;
        private RecyclerView.LayoutManager lm_searchApps;
        private AppCardHorizontalAdapter a_searchApps;

        public SearchFragment()
        {
            searchAppsList = new List<AppToGet>();
        }

        public override void OnCreate(Bundle savedInstanceState)
        {

            // Create your fragment here
            a_searchApps = new AppCardHorizontalAdapter(searchAppsList);
            a_searchApps.onItemClick += A_searchApps_onItemClick;
            base.OnCreate(savedInstanceState);
        }


        private void A_searchApps_onItemClick(object sender, EventArgs e)
        {
            Intent intent = new Intent(Context, typeof(AppDetails_Activity));
            intent.PutExtra("app", JsonConvert.SerializeObject(sender));
            this.StartActivity(intent);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view =  inflater.Inflate(Resource.Layout.searchFragment, container, false);
            rv_searchApps = view!.FindViewById<RecyclerView>(Resource.Id.search_recyclerView);
            lm_searchApps = new LinearLayoutManager(Context, LinearLayoutManager.Vertical, false);
            SpacingDecorator verticalDecorator = new SpacingDecorator(5, 5);
            rv_searchApps.SetLayoutManager(lm_searchApps);
            rv_searchApps.AddItemDecoration(verticalDecorator);
            rv_searchApps.SetAdapter(a_searchApps);
            return view;
        }

        public async void newQuery(string query)
        {
            using (var http = new RestClient("https://api.appstore.renatoventura.pt"))
            {
                var request = new RestRequest("app", Method.Get);
                request.AddQueryParameter("sortBy", "rating");
                request.AddQueryParameter("pageSize", "15");
                request.AddQueryParameter("search", query);
                var res = await http.ExecuteAsync<AppsSearchResponse>(request);
                if (res.IsSuccessful)
                {
                    searchAppsList.Clear();
                    searchAppsList.AddRange(res.Data.results);
                    a_searchApps.NotifyDataSetChanged();
                }

            }
        }
    }
}