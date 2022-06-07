using Altdroid.Logic.Adapters;
using Altdroid.Logic.Models;
using Altdroid.Logic.Views;
using Altdroid.Models;
using Android.App;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace Altdroid.Logic.Activities
{
    [Activity(Label ="Comments",Theme = "@style/AppTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Comments_Activity:AppCompatActivity
    {
        private AppToGet app;
        private RecyclerView rv_Comments;
        private RecyclerView.LayoutManager lm_Comments;
        private commentAdapter a_Comments;

        private TextView rating;
        private ImageView backBtn;

        private List<ApplicationRatingToGet> comments;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            app = JsonConvert.DeserializeObject<AppToGet>(Intent.GetStringExtra("app"));
            SetContentView(Resource.Layout.activity_comment);
            rating = FindViewById<TextView>(Resource.Id.comments_Rating);
            rating.Text = app.ratingAverage.ToString();
            backBtn = FindViewById<ImageView>(Resource.Id.comments_backArrow);
            
            backBtn.Click += BackBtn_Click;
            rv_Comments = FindViewById<RecyclerView>(Resource.Id.comments_rvComments);
            comments = new List<ApplicationRatingToGet>();
            a_Comments = new commentAdapter(comments);
            lm_Comments = new LinearLayoutManager(this, LinearLayoutManager.Vertical,false);
            rv_Comments.SetLayoutManager(lm_Comments);
            rv_Comments.SetAdapter(a_Comments);
            rv_Comments.AddItemDecoration(new SpacingDecorator(5, 5));
            getComments();
        }


        private void BackBtn_Click(object sender, EventArgs e)
        {
            base.OnBackPressed();
        }

        private async void getComments()
        {
            using (var http = new RestClient("https://api.appstore.renatoventura.pt"))
            {
                var request = new RestRequest("AppRatings/"+app.applicationGuid);
                request.Method = Method.Get;
                request.AddHeader("Authorization", "bearer "+ await SecureStorage.GetAsync("token"));
                var res = await http.GetAsync<List<ApplicationRatingToGet>>(request);
                comments.Clear();
                comments.AddRange(res);
                a_Comments.NotifyDataSetChanged();
            }
        }
    }
}