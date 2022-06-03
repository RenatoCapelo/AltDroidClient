using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AndroidX.RecyclerView.Widget;
using Altdroid.Logic.Views;
using Altdroid.Models;
using Square.Picasso;

namespace Altdroid.Adapters
{
    public class AppCardVerticalAdapter : RecyclerView.Adapter
    {
        public List<AppToGet> apps;
        public event EventHandler onItemClick;
        public AppCardVerticalAdapter(List<AppToGet> _apps)
        {
            apps = _apps;
        }
        

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View item = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.appVerticalCard,parent,false);
            

            ImageView image = item.FindViewById<ImageView>(Resource.Id.appVertCard_MainImage);
            TextView Title = item.FindViewById<TextView>(Resource.Id.appVertCard_Title);
            TextView Publisher = item.FindViewById<TextView>(Resource.Id.appVertCard_Publisher);
            TextView Rating = item.FindViewById<TextView>(Resource.Id.appVertCard_Rating);

            AppVerticalCardView cardView = new AppVerticalCardView(item)
            {
                image = image,
                Title = Title,
                Publisher = Publisher,
                Rating = Rating
            };
            
            return cardView;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            AppVerticalCardView cardViewHolder = holder as AppVerticalCardView;
            if (!holder.ItemView.HasOnClickListeners)
            {
                holder.ItemView.Click += (o, e) =>
                {
                    var item = apps[position];
                    onItemClick.Invoke(item, e);
                };
            }

            cardViewHolder.Title.Text = apps[position].title;
            cardViewHolder.Publisher.Text = apps[position].developer.devName;
            cardViewHolder.Rating.Text = apps[position].ratingAverage.ToString();
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