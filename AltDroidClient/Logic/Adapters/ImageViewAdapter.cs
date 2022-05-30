using Altdroid;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using App1.Logic.Views;
using Square.Picasso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App1.Logic.Adapters
{

    internal class ImageViewAdapter : RecyclerView.Adapter
    {
        List<string> imageLinks;
        public override int ItemCount => imageLinks.Count;

        public ImageViewAdapter(List<string> _imageLinks)
        {
            imageLinks = _imageLinks;
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var img = holder as imageView;
            Picasso
                .Get()
                .Load($"https://api.appstore.renatoventura.pt/storage"+imageLinks[position])
                .Error(Resource.Drawable.noImage)
                .Into(img.image);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View item = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.imageView,parent,false);
            ImageView image = item.FindViewById<ImageView>(Resource.Id.imgView_img);

            var imgView = new imageView(item)
            {
                image = image,
            };

            return imgView;
        }
    }
}