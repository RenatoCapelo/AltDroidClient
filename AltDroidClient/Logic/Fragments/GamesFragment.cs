using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;

namespace App1.Logic.Fragments
{
    public class GamesFragment:Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.games_fragment,container,false);
            return view;
        }
    }
}