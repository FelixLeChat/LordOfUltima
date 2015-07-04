using System.Collections;
using Android.Content;
using Android.Views;
using ListViewDragDropSample.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(ViewCell), typeof(MyViewCellRenderer))]
namespace ListViewDragDropSample.Droid
{
	public class MyViewCellRenderer : ViewCellRenderer
	{
		public ListView ParentListView { get; set; }

		public IList Items { get; set; }

		protected override View GetCellCore(Cell item, View convertView, ViewGroup parent, Context context)
		{
			ParentListView = item.ParentView as ListView;

			if (ParentListView != null)
			{
				Items = ParentListView.ItemsSource as IList;
			}

			var cellcore = base.GetCellCore(item, convertView, parent, context);

			cellcore.Drag -= CellcoreOnDrag;
			cellcore.Drag += CellcoreOnDrag;

			return cellcore;
		}

		private void CellcoreOnDrag(object sender, View.DragEventArgs args)
		{
			ViewGroup = sender as ViewGroup;

			if (ViewGroup != null)
			{
				ListView = ViewGroup.Parent.Parent as Android.Widget.ListView;
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
							FirstIndex = ListView.IndexOfChild(ViewGroup.Parent as View);
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
						SecondIndex = ListView.IndexOfChild(ViewGroup.Parent as View);
					}

					if (FirstIndex != -1)
					{
						var firstItem = Items[FirstIndex];

						if (firstItem != null)
						{
							Items.RemoveAt(FirstIndex);
							Items.Insert(SecondIndex, firstItem);

							ParentListView.ItemsSource = null;
							ParentListView.ItemsSource = Items;
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

		public Android.Widget.ListView ListView { get; set; }

		public ViewGroup ViewGroup { get; set; }

		private static int _firstIndex = -1;
		private static int _secondIndex = -1;

		public static int FirstIndex
		{
			get { return _firstIndex; }
			set { _firstIndex = value; }
		}
		public static int SecondIndex
		{
			get { return _secondIndex; }
			set { _secondIndex = value; }
		}
	}
}