using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Altdroid.Logic.Models;
using Altdroid.Logic.Views;

namespace Altdroid.Logic.Adapters
{
    internal class commentAdapter : RecyclerView.Adapter
    {
        public List<ApplicationRatingToGet> comments;
        public commentAdapter(List<ApplicationRatingToGet> _comments)
        {
            comments = _comments;
        }
        public override int ItemCount {
            get { return comments.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var commentViewHolder = holder as commentView;
            if (!holder.ItemView.HasOnClickListeners)
            {
                holder.ItemView.Click += (o, e) =>
                {
                    commentViewHolder.comment.SetMaxLines(commentViewHolder.comment.MaxLines == 2 ? 25 : 2);
                };
            }
            commentViewHolder.author.Text = comments[position].Author.name;
            commentViewHolder.comment.Text = comments[position].Comment;

            switch (comments[position].Rating)
            {
                case 1:
                    commentViewHolder.star0.SetImageResource(Resource.Mipmap.star_filled);
                    break;
                case 2:
                    commentViewHolder.star0.SetImageResource(Resource.Mipmap.star_filled);
                    commentViewHolder.star1.SetImageResource(Resource.Mipmap.star_filled);
                    break;
                case 3:
                    commentViewHolder.star0.SetImageResource(Resource.Mipmap.star_filled);
                    commentViewHolder.star1.SetImageResource(Resource.Mipmap.star_filled);
                    commentViewHolder.star2.SetImageResource(Resource.Mipmap.star_filled);
                    break;
                case 4:
                    commentViewHolder.star0.SetImageResource(Resource.Mipmap.star_filled);
                    commentViewHolder.star1.SetImageResource(Resource.Mipmap.star_filled);
                    commentViewHolder.star2.SetImageResource(Resource.Mipmap.star_filled);
                    commentViewHolder.star3.SetImageResource(Resource.Mipmap.star_filled);
                    break;
                case 5:
                    commentViewHolder.star0.SetImageResource(Resource.Mipmap.star_filled);
                    commentViewHolder.star1.SetImageResource(Resource.Mipmap.star_filled);
                    commentViewHolder.star2.SetImageResource(Resource.Mipmap.star_filled);
                    commentViewHolder.star3.SetImageResource(Resource.Mipmap.star_filled);
                    commentViewHolder.star4.SetImageResource(Resource.Mipmap.star_filled);
                    break;
            }

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View item = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.commentView, parent, false);

            ImageView star0 = item.FindViewById<ImageView>(Resource.Id.comment_star0);
            ImageView star1 = item.FindViewById<ImageView>(Resource.Id.comment_star1);
            ImageView star2 = item.FindViewById<ImageView>(Resource.Id.comment_star2);
            ImageView star3 = item.FindViewById<ImageView>(Resource.Id.comment_star3);
            ImageView star4 = item.FindViewById<ImageView>(Resource.Id.comment_star4);
            TextView author = item.FindViewById<TextView>(Resource.Id.comment_Author);
            TextView comment = item.FindViewById<TextView>(Resource.Id.comment_Message);

            commentView commentView = new commentView(item)
            {
                star0 = star0,
                star1 = star1,
                star2 = star2,
                star3 = star3,
                star4 = star4,
                author = author,
                comment = comment
            };

            return commentView;
        }
    }
}