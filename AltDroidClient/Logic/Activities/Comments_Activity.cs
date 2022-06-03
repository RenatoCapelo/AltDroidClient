using Android.OS;
using AndroidX.AppCompat.App;

namespace Altdroid.Logic.Activities
{
    public class Comments_Activity:AppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_comment);
        }
    }
}