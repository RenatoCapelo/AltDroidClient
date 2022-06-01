using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Altdroid.Models;
using Altdroid.Logic.Services;
using Newtonsoft.Json;
using Square.Picasso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.IO;
using AndroidX.Core.Content;
using Xamarin.Essentials;
using Android.Text.Method;
using AndroidX.RecyclerView.Widget;
using RestSharp;
using App1.Logic.Models;
using App1.Logic.Adapters;
using Altdroid.Logic.Views;

namespace Altdroid.Logic.Activities
{
    [Activity(Label = "AppDetails_Activity",ScreenOrientation =Android.Content.PM.ScreenOrientation.Portrait)]
    public class AppDetails_Activity : AppCompatActivity
    {
        AppToGet app;

        private ImageView btnBack;
        private TextView title;
        private TextView description;
        private TextView price;
        private ImageView logo;
        private Button btnDownload;
        private RecyclerView rv_Photos;
        private RecyclerView.LayoutManager lm_Photos;
        private ImageViewAdapter a_Photos;

        private List<string> _photos;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            app = JsonConvert.DeserializeObject<AppToGet>(Intent.GetStringExtra("app"));
            SetContentView(Resource.Layout.AppDetails);
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
            while (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
            }
            btnBack = FindViewById<ImageView>(Resource.Id.appDetails_backArrow);
            btnBack.Click += BtnBack_Click;
            title = FindViewById<TextView>(Resource.Id.appDetails_Title);
            price = FindViewById<TextView>(Resource.Id.appDetails_Price);
            logo = FindViewById<ImageView>(Resource.Id.appDetails_logo);
            description = FindViewById<TextView>(Resource.Id.appDetails_Description);
            rv_Photos = FindViewById<RecyclerView>(Resource.Id.appDetails_Photos);
            _photos = new List<string>();
            a_Photos = new ImageViewAdapter(_photos);
            lm_Photos = new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false);
            rv_Photos.SetLayoutManager(lm_Photos);
            rv_Photos.SetAdapter(a_Photos);
            rv_Photos.AddItemDecoration(new SpacingDecorator(0, 0, 5, 5));
            getImages();
            description = FindViewById<TextView>(Resource.Id.appDetails_Description);
            description.MovementMethod = new ScrollingMovementMethod();
            btnDownload = FindViewById<Button>(Resource.Id.appDetails_Download);
            btnDownload.Click += BtnDownload_Click;

            title.Text = app.title;
            price.Text = "Gratuito";
            description.Text = app.description;
            logo.SetImageResource(Resource.Drawable.noImage);
            if(app.Icon.HasValue)
            {
                var url = $"https://api.appstore.renatoventura.pt/storage/apps/{app.developer.devGuid}/{app.applicationGuid}/photos/{app.Icon}.png";
                Picasso
                    .Get()
                    .Load(url)
                    .Error(Resource.Drawable.trypsterLogo)
                    .Into(logo);
            }
            // Create your application here
        }

        private void BtnDownload_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            ((Button)sender).Text = "A Descarregar...";
            var downloader = new FileDownloader();
            downloader.DownloadFile($"https://api.appstore.renatoventura.pt/storage/apps/{app.developer.devGuid}/{app.applicationGuid}/{app.fileName}",$"{app.applicationGuid}");
            downloader.OnFileDownloaded += Downloader_OnFileDownloaded;
        }

        private void Downloader_OnFileDownloaded(object sender, Events.DownloadEventArgs e)
        {
            if (e.FileSaved)
            {
                Intent appInstall = new Intent(Intent.ActionView);
                try
                {
                    var pathToInstall = new Java.IO.File(Android.App.Application.Context.GetExternalFilesDir("").AbsolutePath, app.applicationGuid.ToString());
                    var toInstall = new Java.IO.File(pathToInstall, app.fileName);

                    Android.Net.Uri apkUri = AndroidX.Core.Content.FileProvider.GetUriForFile(Application.Context,
                    AppInfo.PackageName + ".provider",
                    toInstall);
                    appInstall.SetDataAndType(apkUri, "application/vnd.android.package-archive");
                    appInstall.SetFlags(ActivityFlags.GrantReadUriPermission);
                    StartActivity(appInstall);

                }
                catch(Exception ex)
                { 
                }
                Toast.MakeText(this, "The app was downloaded!", ToastLength.Long).Show();
            }
            else
            {
                Toast.MakeText(this,"There was an error downloading the app", ToastLength.Long).Show();
            }
        }
        private async void getImages()
        {
            using (var client = new RestClient("https://api.appstore.renatoventura.pt"))
            {
                var request = new RestRequest($"App/Photos/{app.applicationGuid}/3",Method.Get);
                var res = await client.ExecuteAsync<List<Photos>>(request);
                _photos.Clear();
                _photos.AddRange(res.Data.Select(o => $"/apps/{app.developer.devGuid}/{app.applicationGuid}/photos/{o.photo}.png").ToList());
                if (_photos.Count == 0) _photos.Add("/static/noImage.png");
                a_Photos.NotifyDataSetChanged();
            }
        }
        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Finish();
        }
    }
}