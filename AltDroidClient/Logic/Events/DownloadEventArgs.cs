﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Altdroid.Logic.Events
{
    public class DownloadEventArgs:EventArgs
    {
        public bool FileSaved = false;
        public string locatedAt = "";
        public DownloadEventArgs(bool fileSaved, string locatedAt)
        {
            FileSaved = fileSaved;
            this.locatedAt = locatedAt;
        }
    }
}