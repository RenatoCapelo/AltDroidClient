using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using App1.Logic.Views;
using App1.Models;
using Square.Picasso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App1.Logic.Adapters
{
    public class AppCardHorizontalAdapter : RecyclerView.Adapter
    {
        public List<AppToGet> apps;
        public event EventHandler onItemClick;
        public AppCardHorizontalAdapter(List<AppToGet> _apps)
        {
            apps = _apps;
        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View item = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.appHorizontalCard, parent, false);

            ImageView image = item.FindViewById<ImageView>(Resource.Id.appHorizontalCard_MainImage);
            TextView Title = item.FindViewById<TextView>(Resource.Id.appHorizontalCard_Title);
            TextView Category = item.FindViewById<TextView>(Resource.Id.appHorizontalCard_Category);
            TextView Rating = item.FindViewById<TextView>(Resource.Id.appHorizontalCard_Rating);

            AppHorizontalCardView cardView = new AppHorizontalCardView(item)
            {
                image = image,
                Title = Title,
                Category = Category,
                Rating = Rating
            };

            return cardView;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            AppHorizontalCardView cardViewHolder = holder as AppHorizontalCardView;
            holder.ItemView.Click += (o, e) =>
            {
                var item = apps[position];
                onItemClick.Invoke(item, e);
            };
            cardViewHolder.Title.Text = apps[position].title;
            cardViewHolder.Category.Text = apps[position].applicationCategory.name;
            cardViewHolder.Rating.Text = $"#{(position+1).ToString()}";
            cardViewHolder.image.SetImageResource(Resource.Drawable.noImage);
            if (apps[position].Icon.HasValue)
            {
                Picasso
                .Get()
                .Load($"https://api.appstore.renatoventura.pt/storage/apps/{apps[position].developer.devGuid}/{apps[position].applicationGuid}/photos/{apps[position].Icon}.png")
                .Error(Resource.Drawable.noImage)
                .Into(cardViewHolder.image);
            }
        }

        public override int ItemCount
        {
            get { return apps.Count; }
        }
    }
}