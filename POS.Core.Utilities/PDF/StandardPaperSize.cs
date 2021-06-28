using iText.Kernel.Geom;

namespace POS.Core.Utilities.PDF
{
    public class StandardPaperSize : PageSize
    {
        //https://stackoverflow.com/questions/1910881/itextsharp-what-is-the-height-of-a-regular-pdf-page-in-pixels
        private static float pointMultiplier = 72.0f;

        public static PageSize C0 = new PageSize(36.1f  * pointMultiplier , 5.11f  * pointMultiplier  );
        public static PageSize C1 = new PageSize(25.51f * pointMultiplier , 36.1f  * pointMultiplier  );
        public static PageSize C2 = new PageSize(18.03f * pointMultiplier , 25.51f * pointMultiplier  );
        public static PageSize C3 = new PageSize(12.76f * pointMultiplier , 18.03f * pointMultiplier  );
        public static PageSize C4 = new PageSize(9.02f  * pointMultiplier , 12.76f * pointMultiplier  );
        public static PageSize C5 = new PageSize(6.42f  * pointMultiplier , 9.02f  * pointMultiplier  );
        public static PageSize C6 = new PageSize(4.49f  * pointMultiplier , 6.38f  * pointMultiplier  );
        public static PageSize C7 = new PageSize(3.19f  * pointMultiplier , 4.49f  * pointMultiplier  );
        public static PageSize C8 = new PageSize(2.24f  * pointMultiplier , 3.19f  * pointMultiplier  );
        public static PageSize C9 = new PageSize(1.57f  * pointMultiplier , 2.24f  * pointMultiplier  );
        public static PageSize C10 = new PageSize(1.1f  * pointMultiplier , 1.57f  * pointMultiplier  );

        public StandardPaperSize(float width, float height) : base(width, height)
        {
        }

        public StandardPaperSize(Rectangle box) : base(box)
        {
        }
    }
}
