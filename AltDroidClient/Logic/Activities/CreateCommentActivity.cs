using Altdroid.Logic.Models;
using Altdroid.Models;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace Altdroid.Logic.Activities
{
    [Activity(Label = "CreateComment", Theme = "@style/AppTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, WindowSoftInputMode = Android.Views.SoftInput.AdjustNothing)]
    public class CreateCommentActivity : AppCompatActivity
    {
        AppToGet app;
        ApplicationRatingToGet oldRating;
        int currentRating;
        List<ImageView> stars;
        EditText commentEditTxt;
        Button btnPublish;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var ratingSelected = Intent.GetIntExtra("rating",0);
            app = JsonConvert.DeserializeObject<AppToGet>(Intent.GetStringExtra("app"));
            oldRating = JsonConvert.DeserializeObject<ApplicationRatingToGet>(Intent.GetStringExtra("previousRatingComment"));
            currentRating = ratingSelected;
            SetContentView(Resource.Layout.createCommentActivity);
            // Create your application here
            stars = new List<ImageView>();
            stars.Add(FindViewById<ImageView>(Resource.Id.newComment_star0));
            stars.Add(FindViewById<ImageView>(Resource.Id.newComment_star1));
            stars.Add(FindViewById<ImageView> (Resource.Id.newComment_star2));
            stars.Add(FindViewById<ImageView>(Resource.Id.newComment_star3));
            stars.Add(FindViewById<ImageView>(Resource.Id.newComment_star4));
            btnPublish = FindViewById<Button>(Resource.Id.newComment_btnPublish);
            btnPublish.Click += BtnPublish_Click;
            commentEditTxt = FindViewById<EditText>(Resource.Id.newComent_comment);
            commentEditTxt.Text = oldRating.Comment;

            foreach (var star in stars)
            {
                if(stars.IndexOf(star) > ratingSelected)
                    star.SetImageResource(Resource.Mipmap.star);
                else
                    star.SetImageResource(Resource.Mipmap.star_filled);
                star.Tag = stars.IndexOf(star).ToString();
                star.Click += Star_Click;
            }
        }

        private async void BtnPublish_Click(object sender, EventArgs e)
        {
            var method = oldRating.Rating == 0 ? Method.Post : Method.Put;
            using (var http = new RestClient("https://api.appstore.renatoventura.pt/"))
            {
                var request = new RestRequest("AppRatings/" + app.applicationGuid, method);
                request.AddHeader("Authorization", "bearer " + await SecureStorage.GetAsync("token"));
                request.AddBody(new { rating = currentRating+1, message = commentEditTxt.Text }, "application/json");
                var res = await http.ExecuteAsync(request);
                if (res.IsSuccessful)
                    Toast.MakeText(this, "Comentário Guardado com sucesso!", ToastLength.Long).Show();
                else
                    Toast.MakeText(this, "Ocorreu um erro, tente novamente", ToastLength.Long).Show();
            }
        }

        private void Star_Click(object sender, EventArgs e)
        {
            var selectedStar = sender as ImageView;
            currentRating = int.Parse(selectedStar.Tag.ToString());
            for (int i = 0; i <= currentRating; i++)
            {
                stars[i].SetImageResource(Resource.Mipmap.star_filled);
            }
            for(int i = 4; i > currentRating; i--)
            {
                stars[i].SetImageResource(Resource.Mipmap.star);
            }
        }

    }
}