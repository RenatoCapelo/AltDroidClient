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
    public class commentView : RecyclerView.ViewHolder
    {
        public View MainView { get; set; }
        public TextView author { get; set; }
        public ImageView star0 { get; set; }
        public ImageView star1 { get; set; }
        public ImageView star2 { get; set; }
        public ImageView star3 { get; set; }
        public ImageView star4 { get; set; }
        public TextView comment { get; set; }
        public commentView(View itemView) : base(itemView)
        {
            MainView = itemView;
        }
    }
}