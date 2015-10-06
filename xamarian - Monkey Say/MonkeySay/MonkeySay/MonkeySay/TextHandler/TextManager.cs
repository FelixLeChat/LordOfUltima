using System;

namespace MonkeySay.TextHandler
{
    class TextManager
    {
        private static TextManager _instanceTextManager;
        public static TextManager Instance
        { get { return _instanceTextManager ?? (_instanceTextManager = new TextManager()); } }

        public void GenerateNewText(object sender, EventArgs e)
        {
            var text = RandomWordGenerator.Word(new RandomTextLength().GetRandomSyllablesCount());

            Text.Instance.StringText = text;
            Text.Instance.SetText();
        }
    }
}
