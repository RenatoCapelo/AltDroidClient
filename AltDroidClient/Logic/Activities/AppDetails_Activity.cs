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
using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;
using Android.Content.PM;
using AndroidX.Core.Util;
using AndroidX.Core.App;

namespace Altdroid.Logic.Activities
{
    [Activity(Label = "App Details", Theme = "@style/AppTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AppDetails_Activity : AppCompatActivity
    {

        const int INSTALL_REQUEST_CODE = 123;

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
        private ImageView comments;

        private TextView rating;
        private TextView classificationText;
        private ImageView starIcon;
        

        private List<string> _photos;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            app = JsonConvert.DeserializeObject<AppToGet>(Intent.GetStringExtra("app"));
            SetContentView(Resource.Layout.AppDetails);
            askPermission();
            btnBack = FindViewById<ImageView>(Resource.Id.appDetails_backArrow);
            btnBack.Click += BtnBack_Click;
            title = FindViewById<TextView>(Resource.Id.appDetails_Title);
            price = FindViewById<TextView>(Resource.Id.appDetails_Price);
            rating = FindViewById<TextView>(Resource.Id.appDetails_Rating);
            comments = FindViewById<ImageView>(Resource.Id.appDetails_Comments);
            classificationText = FindViewById<TextView>(Resource.Id.appDetails_classificationText);
            starIcon = FindViewById<ImageView>(Resource.Id.appDetails_starIcon);
            rating.Text = app.ratingAverage.ToString();
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
            comments.Click += Comments_Click;
            var exists = packageExists(app.packageName);
            if (exists)
            {
                btnDownload.Enabled = false;
                btnDownload.Text = "Descarregado";
            }
            else
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
                    .Error(Resource.Drawable.noImage)
                    .Into(logo);
            }
            // Create your application here
        }

        private void Comments_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(Comments_Activity));
            Pair[] pairs = new Pair[]
            {
                new Pair(classificationText,"classificationTransition"),
                new Pair(rating,"rating_valueTransition"),
                new Pair(starIcon,"star_iconTransition")
            };
            intent.PutExtra("app",JsonConvert.SerializeObject(app));
            ActivityOptionsCompat options = ActivityOptionsCompat.MakeSceneTransitionAnimation(this, pairs);
            StartActivity(intent,options.ToBundle());
        }

        private async void BtnDownload_Click(object sender, EventArgs e)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
            if(status != PermissionStatus.Granted)
                askPermission();
            else
            {
                ((Button)sender).Enabled = false;
                ((Button)sender).Text = "A Descarregar...";
                var downloader = new FileDownloader();
                downloader.DownloadFile($"https://api.appstore.renatoventura.pt/storage/apps/{app.developer.devGuid}/{app.applicationGuid}/{app.fileName}",$"{app.applicationGuid}");
                downloader.OnFileDownloaded += Downloader_OnFileDownloaded;
            }
        }

        private void Downloader_OnFileDownloaded(object sender, Events.DownloadEventArgs e)
        {
            if (e.FileSaved)
            {
                

                //Intent appInstall = new Intent(Intent.ActionView);
                try
                {
                    var pathToInstall = new Java.IO.File(Android.App.Application.Context.GetExternalFilesDir("").AbsolutePath, app.applicationGuid.ToString());
                    var toInstall = new Java.IO.File(pathToInstall, app.fileName);

                    Android.Net.Uri apkUri = AndroidX.Core.Content.FileProvider.GetUriForFile(Application.Context,
                    AppInfo.PackageName + ".provider",
                    toInstall);
                    //appInstall.SetDataAndType(apkUri, "application/vnd.android.package-archive");
                    //appInstall.SetFlags(ActivityFlags.GrantReadUriPermission);
                    //StartActivity(appInstall);
                    Intent intent = new Intent(Intent.ActionInstallPackage);
                    intent.SetDataAndType(apkUri, "application/vnd.android.package-archive");
                    intent.SetFlags(ActivityFlags.GrantReadUriPermission);
                    intent.PutExtra(Intent.ExtraReturnResult, true);
                    StartActivityForResult(intent, INSTALL_REQUEST_CODE);
                }
                catch (Exception)
                {
                    Toast.MakeText(this,"There was an error downloading the app", ToastLength.Long).Show();
                }
            }
            else
            {
                Toast.MakeText(this,"There was an error downloading the app", ToastLength.Long).Show();
            }
        }
        protected override async void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            switch (requestCode)
            {
                case INSTALL_REQUEST_CODE:
                    using (var http = new RestClient("https://api.appstore.renatoventura.pt"))
                    {
                        var request = new RestRequest("App/Download/"+app.applicationGuid,Method.Post);
                        request.AddHeader("Authorization", "bearer "+await SecureStorage.GetAsync("token"));
                        var task = http.ExecuteAsync(request);
                        Task.WaitAll(task);
                    }
                    if (packageExists(app.packageName))
                    {
                        btnDownload.Text = "Instalado!";
                        Toast.MakeText(this,"Instalado com sucesso!",ToastLength.Long).Show();
                    }
                    else
                    {
                        btnDownload.Text = "Download";
                        btnDownload.Enabled = true;
                        Toast.MakeText(this,"Ocorreu um erro na instalação :/",ToastLength.Long).Show();
                    }
                    break;
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
            OnBackPressed();
        }
        public override void OnBackPressed()
        {
            this.Finish();
        }

        private async void askPermission()
        {
                await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
        }

        public bool packageExists(String targetPackage)
        {
            PackageManager pm = this.PackageManager;
            try
            {
                PackageInfo info = pm.GetPackageInfo(targetPackage, PackageInfoFlags.MetaData);
            }
            catch (PackageManager.NameNotFoundException e)
            {
                return false;
            }
            return true;
        }
    }
}