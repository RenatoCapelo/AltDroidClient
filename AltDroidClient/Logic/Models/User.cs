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

namespace App1.Models
{
    internal class User
    {
        public string guid { get; set; }
        public string email { get; set; }
        public string photoGuid { get; set; }
        public string name { get; set; }
        public DateTime dob { get; set; }
        public Gender gender { get; set; }
    }
}