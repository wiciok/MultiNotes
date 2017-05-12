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

namespace MultiNotes.XAndroid
{
    class NotesAdapter : BaseAdapter
    {
        private List<Note> notesList;

        public NotesAdapter(Context context, int resourceId, System.Collections.IList objects)
            : base(context, resourceId, objects)
        { 
            /*
            resource = resourceId;
            inflater = LayoutInflater.from(ctx);
            context = ctx;
            */
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            /*
            return base.GetView(position, convertView, parent);
            */
        }
    }
}