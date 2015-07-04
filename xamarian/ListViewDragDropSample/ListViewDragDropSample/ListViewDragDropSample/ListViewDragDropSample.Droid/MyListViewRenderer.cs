using Android.Content;
using ListViewDragDropSample.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ListView), typeof(MyListViewRenderer))]
namespace ListViewDragDropSample.Droid
{
	public class MyListViewRenderer : ListViewRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
		{
			base.OnElementChanged(e);

			Control.ItemLongClick += (s, args) =>
			{
				ClipData data = ClipData.NewPlainText("List", args.Position.ToString());
				MyDragShadowBuilder myShadownScreen = new MyDragShadowBuilder(args.View);
				args.View.StartDrag(data, myShadownScreen, null, 0);
			};
		}
	}
}