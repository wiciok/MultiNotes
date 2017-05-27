using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using MultiNotes.Core;
using MultiNotes.Model;

namespace MultiNotes.XAndroid.Model
{
    public class UserHeader
    {

        public static readonly UserHeader Empty = new UserHeader("", "");


        // Non-static part starts here

        private string id;
        private string emailAddress;


        public UserHeader(User user) : this(user.Id, user.EmailAddress) { }

        public UserHeader(string id, string emailAddress)
        {
            this.id = id;
            this.emailAddress = emailAddress;
        }


        public string Id { get { return id; } }
        public string EmailAddress { get { return emailAddress; } }

    }
}