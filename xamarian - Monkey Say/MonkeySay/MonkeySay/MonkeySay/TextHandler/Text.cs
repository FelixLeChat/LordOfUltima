using Xamarin.Forms;

namespace MonkeySay.TextHandler
{
    class Text
    {
        private static Text _instanceText;
        public static Text Instance 
        { get { return _instanceText ?? (_instanceText = new Text()); } }

        public string StringText { get; set; }
        public Label LabelText { get; set; }

        public void SetText()
        {
            LabelText.Text = StringText;
        }
    }
}
