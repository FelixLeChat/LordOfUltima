using MonkeySay.TextHandler;
using Xamarin.Forms;

namespace MonkeySay.Views
{
	public class MonkeyPage : ContentPage
	{
        public MonkeyPage()
		{
            // New Layout
            var layout = new RelativeLayout();

            // fullscreen image
            var myImage = new Image()
            {
                Source = ImageSource.FromResource("MonkeySay.background.png"),
                Aspect = Aspect.AspectFill,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            layout.Children.Add(myImage,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent(parent => parent.Width),
                Constraint.RelativeToParent(parent => parent.Height));

            // Label for text
            var label = new Label()
            {
                FontSize = 20,
                TextColor = Color.White,
                Text = "",
                HorizontalOptions = LayoutOptions.Center
            };
            layout.Children.Add(label,
                Constraint.RelativeToParent(parent => parent.Width/4),
                Constraint.RelativeToParent(parent => parent.Height/4),
                Constraint.RelativeToParent(parent => parent.Width),
                Constraint.RelativeToParent(parent => parent.Height));
            // set text label
            TextHandler.Text.Instance.LabelText = label;

            // Button for new text
            var button = new Button()
            {
                Text = "New Text"
            };
            button.Clicked += TextManager.Instance.GenerateNewText;
            layout.Children.Add(button,
                Constraint.RelativeToParent(parent => parent.Width / 4),
                Constraint.RelativeToParent(parent => parent.Height / (1.6)),
                Constraint.RelativeToParent(parent => parent.Width / 2),
                Constraint.RelativeToParent(parent => parent.Height / 10));

            Content = layout;
		}
	}
}
