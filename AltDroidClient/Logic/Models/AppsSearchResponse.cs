using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Altdroid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Altdroid.Logic.Models
{
    public class AppsSearchResponse
    {
        public int pages { get; set; }
        public int currentPages { get; set; }
        public int count { get; set; }
        public List<AppToGet> results { get; set; }
    }
}