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

namespace App1.Logic.Fragments
{
    
    public class AppsFragment:Fragment
    {
        private List<AppToGet> featuredAppsList;
        private RecyclerView rv_featuredApps;
        private RecyclerView.LayoutManager lm_featuredApps;
        private RecyclerView.Adapter a_featuredApps;


        private List<AppToGet> bestRatedAppsList;
        private RecyclerView rv_bestRatedApps;
        private RecyclerView.LayoutManager lm_bestApps;
        private RecyclerView.Adapter a_bestApps;


        private List<AppToGet> newAppsList;
        private RecyclerView rv_newApps;
        private RecyclerView.LayoutManager lm_newApps;
        private RecyclerView.Adapter a_newApps;
        public override void OnCreate(Bundle savedInstanceState)
        {
            featuredAppsList = new List<AppToGet>();
            bestRatedAppsList = new List<AppToGet>();
            newAppsList = new List<AppToGet>();

            a_featuredApps = new AppCardVerticalAdapter(this.featuredAppsList);
            a_bestApps = new AppCardHorizontalAdapter(this.bestRatedAppsList);
            a_newApps = new AppCardVerticalAdapter(this.newAppsList);
            base.OnCreate(savedInstanceState);
            
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            GetFeaturedApps();
            GetBestRatedApps();
            GetNewApps();
            View view = inflater.Inflate(Resource.Layout.apps_fragment, container,false);
            
            rv_featuredApps = view!.FindViewById<RecyclerView>(Resource.Id.apps_FeaturedAppsRecyclerView);
            rv_newApps = view!.FindViewById<RecyclerView>(Resource.Id.apps_NewAppsRecyclerView);
            rv_bestRatedApps = view!.FindViewById<RecyclerView>(Resource.Id.apps_TopRatedRecyclerView);
            

            lm_featuredApps = new LinearLayoutManager(Context,LinearLayoutManager.Horizontal,false);
            lm_newApps = new LinearLayoutManager(Context, LinearLayoutManager.Horizontal, false);
            lm_bestApps = new LinearLayoutManager(Context, LinearLayoutManager.Vertical, false);

            rv_featuredApps.SetLayoutManager(lm_featuredApps);
            rv_newApps.SetLayoutManager(lm_newApps);
            rv_bestRatedApps.SetLayoutManager(lm_bestApps);
            
            rv_featuredApps.SetAdapter(a_featuredApps);
            rv_newApps.SetAdapter(a_newApps);
            rv_bestRatedApps.SetAdapter(a_bestApps);
            
            return view;
        }

        public async void GetFeaturedApps()
        {
            using (var http = new RestClient("https://api.appstore.renatoventura.pt"))
            {
                var request = new RestRequest("app", Method.Get);
                request.AddQueryParameter("masterCategory", "1");
                request.AddQueryParameter("sortBy", "downloads");
                var response = await http.ExecuteAsync<AppsSearchResponse>(request);
                if (response.IsSuccessful)
                {
                    featuredAppsList.Clear();
                    featuredAppsList.AddRange(response.Data.results);
                    a_featuredApps.NotifyDataSetChanged();
                }
            }
        }

        public async void GetNewApps()
        {
            using (var http = new RestClient("https://api.appstore.renatoventura.pt"))
            {
                var request = new RestRequest("app", Method.Get);
                request.AddQueryParameter("masterCategory", "1");
                request.AddQueryParameter("sortBy", "date");
                var response = await http.ExecuteAsync<AppsSearchResponse>(request);
                if (response.IsSuccessful)
                {
                    newAppsList.Clear();
                    newAppsList.AddRange(response.Data.results);
                    a_newApps.NotifyDataSetChanged();
                }
            }
        }

        public async void GetBestRatedApps()
        {
            using (var http = new RestClient("https://api.appstore.renatoventura.pt"))
            {
                var request = new RestRequest("app",Method.Get);
                request.AddQueryParameter("masterCategory", 1);
                request.AddQueryParameter("sortBy", "rating");
                var response = await http.ExecuteAsync<AppsSearchResponse>(request);
                if(response.IsSuccessful)
                {
                    bestRatedAppsList.Clear();
                    bestRatedAppsList.AddRange(response.Data.results);
                    a_bestApps.NotifyDataSetChanged();
                }
            }
        }
    }
}