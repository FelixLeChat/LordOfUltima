using System;
using System.Reflection.Emit;

using Xamarin.Forms;
using Label = Xamarin.Forms.Label;

namespace MonkeySay.Views
{
	public class BonusReorganisePage : ContentPage
	{
		public BonusReorganisePage ()
		{
            // Click Handler
            var tapGestureRecognizer = new TapGestureRecognizer();
		    tapGestureRecognizer.Tapped += handleTap;

            // layout
            var layout = new RelativeLayout();

		    var letterCount = TextHandler.Text.Instance.StringText;
            if(letterCount != null)
		    for (var i = 0; i < letterCount.Length; i++)
		    {
		        var label = new Label()
		        {
		            Text = "W"
		        };

                layout.Children.Add(label,
                    Constraint.RelativeToParent(parent => i*(parent.Width/letterCount.Length)),
                    Constraint.RelativeToParent(parent => parent.Height/2),
                    Constraint.RelativeToParent(parent => parent.Width),
                    Constraint.RelativeToParent(parent => parent.Height/2 + 30));
		    }

		    Content = layout;
		}

	    private void handleTap(object o, EventArgs e)
	    {
	        
	    }
	}
}
