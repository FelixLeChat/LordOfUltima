using System;
using System.Collections.Generic;
using System.Linq;
using MonkeySay.TextHandler;
using Xamarin.Forms;
using Label = Xamarin.Forms.Label;

namespace MonkeySay.Views
{
	public class BonusReorganisePage : ContentPage
	{
	    private readonly Label _newWordLabel;
	    private readonly List<Label> _clickedLabels = new List<Label>();
	    private readonly Image _undoButton;
	    private readonly Image _confirmButton;

		public BonusReorganisePage ()
		{
            // Letter Click Handler
            var letterTap = new TapGestureRecognizer();
		    letterTap.Tapped += handleTap;

            // Undo Click Handler
            var undoTap = new TapGestureRecognizer();
            undoTap.Tapped += handleUndo;

            // Confirm Click Handler
            var confirmTap = new TapGestureRecognizer();
		    confirmTap.Tapped += handleConfirm;

            // layout
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

            // label for new word
		    var label = new Label()
		    {
                FontSize = 20,
                TextColor = Color.White,
                Text = "",
                HorizontalOptions = LayoutOptions.Center
		    };
            layout.Children.Add(label,
                Constraint.RelativeToParent(parent => parent.Width / 4),
                Constraint.RelativeToParent(parent => parent.Height / 4),
                Constraint.RelativeToParent(parent => parent.Width),
                Constraint.RelativeToParent(parent => parent.Height));
		    _newWordLabel = label;

            // setup the clickable label containing each letter
		    var letterCount = TextHandler.Text.Instance.StringText;
            if(letterCount != null)
		    for (var i = 0; i < letterCount.Length; i++)
		    {
		        label = new Label()
		        {
                    FontSize = 20,
                    TextColor = Color.White,
                    HorizontalOptions = LayoutOptions.Center,
		            Text = letterCount[i].ToString()
		        };
                
                label.GestureRecognizers.Add(letterTap);

		        var i1 = i/7;
		        var i2 = i%7;
		        layout.Children.Add(label,
                    Constraint.RelativeToParent(parent => parent.Width / 8 + ((i2 + 1) * 35)),
                    Constraint.RelativeToParent(parent => parent.Height / 3 + i1 * 30),
                    Constraint.RelativeToParent(parent => 30),
                    Constraint.RelativeToParent(parent => 30));
		    }

            // Undo Button
		    var undoImage = new Image()
		    {
                Source = ImageSource.FromResource("MonkeySay.undo_button.png"),
                Aspect = Aspect.AspectFit,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 100,
                WidthRequest = 100,
                IsVisible = false
		    };
		    _undoButton = undoImage;
            undoImage.GestureRecognizers.Add(undoTap);
            layout.Children.Add(undoImage,
                Constraint.RelativeToParent(parent => parent.Width / 20),
                Constraint.RelativeToParent(parent => parent.Height / 1.6),
                Constraint.RelativeToParent(parent => 100),
                Constraint.RelativeToParent(parent => 100));

            // Confirm Button
		    var confirmButton = new Image()
		    {
                Source = ImageSource.FromResource("MonkeySay.confirm_button.png"),
                Aspect = Aspect.AspectFit,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 100,
                WidthRequest = 100,
                IsVisible = false
            };
		    _confirmButton = confirmButton;
            confirmButton.GestureRecognizers.Add(confirmTap);
            layout.Children.Add(confirmButton,
                Constraint.RelativeToParent(parent => parent.Width / 20 + 90),
                Constraint.RelativeToParent(parent => parent.Height / 1.6),
                Constraint.RelativeToParent(parent => 100),
                Constraint.RelativeToParent(parent => 100));

		    Content = layout;
		}

	    private void handleTap(object obj, EventArgs e)
	    {
            if (obj.GetType() == typeof(Label))
            {
                var label = (Label) obj;

                // Hide the label
                label.IsVisible = false;
                _newWordLabel.Text += label.Text;
                
                if(!_clickedLabels.Contains(label))
                    _clickedLabels.Add(label);

                // Show the undo button
                _undoButton.IsVisible = true;
            }

            // Confirm button visibility
	        if (_newWordLabel.Text.Length == Text.Instance.StringText.Length)
	        {
	            _confirmButton.IsVisible = true;
	        }
	    }

	    private void handleUndo(object obj, EventArgs e)
	    {
	        if (_clickedLabels.Count > 0)
	        {
	            var label = _clickedLabels.Last();

                // show the last label
	            label.IsVisible = true;

                // remove last letter in word label
                if(_newWordLabel.Text.Length > 0)
                    _newWordLabel.Text = _newWordLabel.Text.Remove(_newWordLabel.Text.Length - 1);

                // remove the label
	            _clickedLabels.Remove(label);

                // Hide confirm
	            _confirmButton.IsVisible = false;
	        }

	        if(_clickedLabels.Count == 0)
	        {
	            _undoButton.IsVisible = false;
	        }
	    }

	    private void handleConfirm(object obj, EventArgs e)
	    {
	        Text.Instance.StringText = _newWordLabel.Text;
            Text.Instance.SetText();
            // Switch back to main page
            App.Instance.ChangePageToMain();
	    }

	}
}
