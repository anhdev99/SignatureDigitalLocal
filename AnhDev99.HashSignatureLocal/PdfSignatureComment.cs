namespace AnhDev99.HashSignatureLocal
{
    using iText.Kernel.Geom;
    using System;

    public class PdfSignatureComment
    {
        public string ID;

       public PdfSignatureComment() { }

        public int Type { get; set; }
        public string Rectangle { get; set; }
        public string Text { get; set; }
        public int Page { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string FontName { get; set; }
        public int FontStyle { get; set; }
        public int FontSize { get; set; }
        public string FontColor { get; set; }
        public string Background { get; set; }
        public string FontWeight { get; set; }
        public byte[] ImageBgBytes
        {
            get
            {
                try
                {
                    return Convert.FromBase64String(Background);
                }
                catch (Exception)
                {
                }
                return null;
            }
        }

        public Rectangle GetRectangle()
        {
            if (string.IsNullOrEmpty(Rectangle))
            {
                return null;
            }
            string[] array = Rectangle.Split(',');
            if (array != null && array.Length == 4)
            {
                try
                {
                    int num = int.Parse(array[0]);
                    int num2 = int.Parse(array[1]);
                    int num3 = int.Parse(array[2]);
                    int num4 = int.Parse(array[3]);
                    return new Rectangle(num, num2, num3, num4);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        public enum Types
        {
            IMAGE = 1,
            TEXT = 2,
        }
    }


}
