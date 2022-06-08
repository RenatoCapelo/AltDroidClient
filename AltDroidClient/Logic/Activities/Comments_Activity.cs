using Altdroid.Logic.Adapters;
using Altdroid.Logic.Models;
using Altdroid.Logic.Views;
using Altdroid.Models;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        List<ImageView> stars;
        private TextView comment;

        private List<ApplicationRatingToGet> comments;
        private ApplicationRatingToGet userComment;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            app = JsonConvert.DeserializeObject<AppToGet>(Intent.GetStringExtra("app"));
            userComment = new ApplicationRatingToGet();
            SetContentView(Resource.Layout.activity_comment);
            rating = FindViewById<TextView>(Resource.Id.comments_Rating);
            rating.Text = app.ratingAverage.ToString();
            backBtn = FindViewById<ImageView>(Resource.Id.comments_backArrow);
            backBtn.Click += BackBtn_Click;
            comment = FindViewById<TextView>(Resource.Id.comments_Comment);
            stars = new List<ImageView>();
            stars.Add(FindViewById<ImageView>(Resource.Id.comments_star0));
            stars.Add(FindViewById<ImageView>(Resource.Id.comments_star1));
            stars.Add(FindViewById<ImageView>(Resource.Id.comments_star2));
            stars.Add(FindViewById<ImageView>(Resource.Id.comments_star3));
            stars.Add(FindViewById<ImageView>(Resource.Id.comments_star4));
            rv_Comments = FindViewById<RecyclerView>(Resource.Id.comments_rvComments);
            comments = new List<ApplicationRatingToGet>();
            a_Comments = new commentAdapter(comments);
            lm_Comments = new LinearLayoutManager(this, LinearLayoutManager.Vertical,false);
            rv_Comments.SetLayoutManager(lm_Comments);
            rv_Comments.SetAdapter(a_Comments);
            rv_Comments.AddItemDecoration(new SpacingDecorator(5, 5));
            getComments();
            await getUserComment();
            comment.Text = String.IsNullOrEmpty(userComment.Comment) ? comment.Text : "Comentário do utilizador: "+userComment.Comment;
            foreach (var star in stars)
            {
                var index = stars.IndexOf(star);
                star.Tag = index.ToString();
                star.Click += Star_Click;
                star.SetImageResource(userComment.Rating > index ? Resource.Mipmap.star_filled : Resource.Mipmap.star);
            }
        }


        private void BackBtn_Click(object sender, EventArgs e)
        {
            base.OnBackPressed();
        }

        private void Star_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(CreateCommentActivity));
            var selectedStar = sender as ImageView;
            var index = int.Parse(selectedStar.Tag.ToString());
            for (int i = 0; i <= index; i++)
            {
                stars[i].SetImageResource(Resource.Mipmap.star_filled);
            }
            for (int i = 4; i > index; i--)
            {
                stars[i].SetImageResource(Resource.Mipmap.star);
            }
            intent.PutExtra("rating", index);
            intent.PutExtra("app", Intent.GetStringExtra("app"));
            intent.PutExtra("previousRatingComment",JsonConvert.SerializeObject(userComment));
            StartActivity(intent);
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

        private async Task getUserComment()
        {
            using (var http = new RestClient("https://api.appstore.renatoventura.pt"))
            {
                var request = new RestRequest("AppRatings/"+app.applicationGuid+"/User");
                request.Method = Method.Get;
                request.AddHeader("Authorization", "bearer " + await SecureStorage.GetAsync("token"));
                var res = await http.GetAsync<List<ApplicationRatingToGet>>(request);
                userComment = res.Count > 0 ? res[0] : userComment;
                return;
            }
        }
    }
}