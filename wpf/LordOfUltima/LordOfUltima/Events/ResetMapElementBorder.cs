namespace LordOfUltima.Events
{
    class ResetMapElementBorder
    {
        private static ResetMapElementBorder _ins;
        public static ResetMapElementBorder Instance
        {
            get { return _ins ?? (_ins = new ResetMapElementBorder()); }
        }

        public void ResetSelectionBorder()
        {
            foreach (Element element in Gameboard.Instance.GetMap())
            {
                element.hideSelectBorder();
            }
        }
    }
}
