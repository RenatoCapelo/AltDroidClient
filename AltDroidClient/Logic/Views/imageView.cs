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

namespace App1.Logic.Views
{
    public class imageView : RecyclerView.ViewHolder
    {
        public View MainView { get; set; }
        public ImageView image { get; set; }
        
        public imageView(View view) : base(view)
        {
            MainView = view;
        }
    }
}