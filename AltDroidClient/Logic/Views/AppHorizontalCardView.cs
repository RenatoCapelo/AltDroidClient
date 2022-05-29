using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Altdroid.Logic.Views
{
    public class AppHorizontalCardView : RecyclerView.ViewHolder
    {
        public TextView Rating { get; set; }
        public View MainView { get; set; }
        public ImageView image { get; set; }
        public TextView Title { get; set; }
        public TextView Category { get; set; }

        public AppHorizontalCardView(View view) : base(view)
        {
            MainView = view;
        }
    }
}