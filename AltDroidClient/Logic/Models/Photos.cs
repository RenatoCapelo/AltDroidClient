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

namespace App1.Logic.Models
{
    internal class Photos
    {
        public int idPhotoType { get; set; }
        public Guid photo { get; set; }
    }
}