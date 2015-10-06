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

namespace ListViewDragDropSample.Droid
{
	public class MyAdapter : BaseAdapter<string>
	{
		List<string> items;

		Activity context;

		public MyAdapter(Activity context, List<string> items)
			: base()
		{
			this.context = context;
			this.items = items;
		}


		#region implemented abstract members of BaseAdapter

		public override long GetItemId(int position)
		{
			return position;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = items[position];

			View view = convertView;

			//if (view == null)
			//{
				view = new TextView(context) {Text = item};

				view.Drag -= view_Drag;
				view.Drag += view_Drag;
			//}

			return view;
		}

		private int _firstIndex = -1;
		private int _secondIndex = -1;

		public int FirstIndex
		{
			get { return _firstIndex; }
			set { _firstIndex = value; }
		}
		public int SecondIndex
		{
			get { return _secondIndex; }
			set { _secondIndex = value; }
		}
		public TextView TextView { get; set; }
		public ListView ListView { get; set; }

		private void view_Drag(object sender, View.DragEventArgs args)
		{
			TextView = sender as TextView;

			if (TextView != null)
			{
				ListView = TextView.Parent as ListView;
			}

			switch (args.Event.Action)
			{
				case DragAction.Started:
					args.Handled = true;
					break;

				case DragAction.Entered:
					args.Handled = true;

					if (ListView != null)
					{
						if (FirstIndex == -1)
						{
							FirstIndex = ListView.IndexOfChild(TextView);
						}
					}

					break;

				case DragAction.Exited:
					args.Handled = true;
					break;

				case DragAction.Drop:
					args.Handled = true;

					if (SecondIndex == -1)
					{
						SecondIndex = ListView.IndexOfChild(TextView);
					}

					if (FirstIndex != -1)
					{
						var firstItem = ListView.GetChildAt(FirstIndex) as TextView;

						if (firstItem != null)
						{
							//ListView.AddView(firstItem, SecondIndex);
							items.RemoveAt(FirstIndex);
							items.Insert(SecondIndex, firstItem.Text);
							context.RunOnUiThread(NotifyDataSetChanged);
						}
					}

					FirstIndex = -1;
					SecondIndex = -1;

					break;
				case DragAction.Ended:
					args.Handled = true;
					break;
			}
		}

		public override int Count
		{
			get { return items.Count; }
		}

		#endregion

		#region implemented abstract members of BaseAdapter

		public override string this[int index]
		{
			get
			{
				return items[index];
			}
		}

		#endregion


		public override void NotifyDataSetChanged()
		{
			base.NotifyDataSetChanged();
		}

		public void Add(string p)
		{
			items.Add(p);
			NotifyDataSetChanged();
		}

		public void Remove(int index)
		{
			items.RemoveAt(index);
			NotifyDataSetChanged();
		}

		public string GetItem(int index)
		{
			return items[index];
		}
	}
}