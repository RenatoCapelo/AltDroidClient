using Android.Content;
using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using Altdroid.Adapters;
using Altdroid.Logic.Activities;
using Altdroid.Logic.Adapters;
using Altdroid.Logic.Models;
using Altdroid.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;

namespace Altdroid.Logic.Fragments
{
    public class GamesFragment:Fragment
    {
        private List<AppToGet> featuredAppsList;
        private RecyclerView rv_featuredApps;
        private RecyclerView.LayoutManager lm_featuredApps;
        private AppCardVerticalAdapter a_featuredGames;


        private List<AppToGet> bestRatedGamesList;
        private RecyclerView rv_bestRatedGames;
        private RecyclerView.LayoutManager lm_bestGames;
        private AppCardHorizontalAdapter a_bestGames;


        private List<AppToGet> newGamesList;
        private RecyclerView rv_newGames;
        private RecyclerView.LayoutManager lm_newGames;
        private AppCardVerticalAdapter a_newGames;
        public override void OnCreate(Bundle savedInstanceState)
        {
            featuredAppsList = new List<AppToGet>();
            bestRatedGamesList = new List<AppToGet>();
            newGamesList = new List<AppToGet>();

            a_featuredGames = new AppCardVerticalAdapter(this.featuredAppsList);
            a_featuredGames.onItemClick += onItemClick;

            a_bestGames = new AppCardHorizontalAdapter(this.bestRatedGamesList);
            a_bestGames.onItemClick += onItemClick;

            a_newGames = new AppCardVerticalAdapter(this.newGamesList);
            a_newGames.onItemClick += onItemClick;

            base.OnCreate(savedInstanceState);

        }
        private void onItemClick(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(Context, typeof(AppDetails_Activity));
            intent.PutExtra("app", JsonConvert.SerializeObject(sender));
            this.StartActivity(intent);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            GetFeaturedApps();
            GetBestRatedApps();
            GetNewApps();
            View view = inflater.Inflate(Resource.Layout.apps_fragment, container, false);

            rv_featuredApps = view!.FindViewById<RecyclerView>(Resource.Id.apps_FeaturedAppsRecyclerView);
            rv_newGames = view!.FindViewById<RecyclerView>(Resource.Id.apps_NewAppsRecyclerView);
            rv_bestRatedGames = view!.FindViewById<RecyclerView>(Resource.Id.apps_TopRatedRecyclerView);


            lm_featuredApps = new LinearLayoutManager(Context, LinearLayoutManager.Horizontal, false);
            lm_newGames = new LinearLayoutManager(Context, LinearLayoutManager.Horizontal, false);
            lm_bestGames = new LinearLayoutManager(Context, LinearLayoutManager.Vertical, false);

            rv_featuredApps.SetLayoutManager(lm_featuredApps);
            rv_newGames.SetLayoutManager(lm_newGames);
            rv_bestRatedGames.SetLayoutManager(lm_bestGames);

            rv_featuredApps.SetAdapter(a_featuredGames);
            rv_newGames.SetAdapter(a_newGames);
            rv_bestRatedGames.SetAdapter(a_bestGames);

            return view;
        }

        public async void GetFeaturedApps()
        {
            using (var http = new RestClient("https://api.appstore.renatoventura.pt"))
            {
                var request = new RestRequest("app", Method.Get);
                request.AddQueryParameter("masterCategory", 2);
                request.AddQueryParameter("sortBy", "downloads");
                var response = await http.ExecuteAsync<AppsSearchResponse>(request);
                if (response.IsSuccessful)
                {
                    featuredAppsList.Clear();
                    featuredAppsList.AddRange(response.Data.results);
                    a_featuredGames.NotifyDataSetChanged();
                }
            }
        }

        public async void GetNewApps()
        {
            using (var http = new RestClient("https://api.appstore.renatoventura.pt"))
            {
                var request = new RestRequest("app", Method.Get);
                request.AddQueryParameter("masterCategory", 2);
                request.AddQueryParameter("sortBy", "date");
                var response = await http.ExecuteAsync<AppsSearchResponse>(request);
                if (response.IsSuccessful)
                {
                    newGamesList.Clear();
                    newGamesList.AddRange(response.Data.results);
                    a_newGames.NotifyDataSetChanged();
                }
            }
        }

        public async void GetBestRatedApps()
        {
            using (var http = new RestClient("https://api.appstore.renatoventura.pt"))
            {
                var request = new RestRequest("app", Method.Get);
                request.AddQueryParameter("masterCategory", 2);
                request.AddQueryParameter("sortBy", "rating");
                var response = await http.ExecuteAsync<AppsSearchResponse>(request);
                if (response.IsSuccessful)
                {
                    bestRatedGamesList.Clear();
                    bestRatedGamesList.AddRange(response.Data.results);
                    a_bestGames.NotifyDataSetChanged();
                }
            }
        }
    }
}