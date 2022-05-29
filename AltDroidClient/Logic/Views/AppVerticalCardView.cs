using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;

namespace Altdroid.Logic.Views
{
    public class AppVerticalCardView:RecyclerView.ViewHolder
    {
        public View MainView { get; set; }
        public ImageView image { get; set; }
        public TextView Title { get; set; }
        public TextView Publisher { get; set; }
        public TextView Rating { get; set; }

        public AppVerticalCardView(View view):base(view)
        {
            MainView = view;
        }
    }
}