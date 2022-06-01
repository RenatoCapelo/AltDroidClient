using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Altdroid.Logic.Views
{
    internal class SpacingDecorator : RecyclerView.ItemDecoration
    {
        private readonly int spaceTop;
        private readonly int spaceBottom;
        private readonly int spaceLeft;
        private readonly int spaceRight;

        public SpacingDecorator(int spaceAllSides)
        {
            this.spaceTop = spaceAllSides;
            this.spaceBottom = spaceAllSides;
            this.spaceLeft = spaceAllSides;
            this.spaceRight = spaceAllSides;
        }
        public SpacingDecorator(int spaceTop=0, int spaceBottom=0, int spaceLeft=0, int spaceRight=0)
        {
            this.spaceTop = spaceTop;
            this.spaceBottom = spaceBottom;
            this.spaceLeft = spaceLeft;
            this.spaceRight = spaceRight;
        }

        public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
        {
            outRect.Top = spaceTop;
            outRect.Right = spaceRight;
            outRect.Left = spaceLeft;
            outRect.Bottom = spaceBottom;
        }
    }
}