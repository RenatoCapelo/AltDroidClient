using Altdroid.Models;
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

namespace Altdroid.Logic.Models
{
    internal class ApplicationRatingToGet
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public User Author { get; set; }
        public string Comment { get; set; }
    }
}