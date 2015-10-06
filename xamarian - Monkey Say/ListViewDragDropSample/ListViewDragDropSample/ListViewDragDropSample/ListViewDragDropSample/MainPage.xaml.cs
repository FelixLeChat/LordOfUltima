using System.Collections.Generic;
using Xamarin.Forms;

namespace ListViewDragDropSample
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

			Items = new List<Item>();

			for (int i = 1; i < 11; i++)
			{
				Items.Add(new Item()
				{
					Title = "Title : " + i,
					Description = "Description : " + i,
				});
			}

			BindingContext = this;
		}

		public List<Item> Items { get; set; }
	}
}
