using System.Collections;

namespace OpenCVDetection.PRM
{
     class BitmapExtensions
    {
        public static Bitmap CropImage(Image originalImage, Rectangle sourceRectangle, Rectangle? destinationRectangle = null)
        {
            if (destinationRectangle == null)
            {
                destinationRectangle = new Rectangle(Point.Empty, sourceRectangle.Size);
            }

            var croppedImage = new Bitmap(destinationRectangle.Value.Width,
                destinationRectangle.Value.Height);
            using (var graphics = Graphics.FromImage(croppedImage))
            {
                graphics.DrawImage(originalImage, destinationRectangle.Value,
                    sourceRectangle, GraphicsUnit.Pixel);
            }
            return croppedImage;
        }

        public static Point[] PixelSearch(Rectangle rect, Color Pixel_Color, int Shade_Variation)
        {
            ArrayList points = new ArrayList();
            Bitmap RegionIn_Bitmap = new Bitmap(rect.Width, rect.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            using (Graphics GFX = Graphics.FromImage(RegionIn_Bitmap))
            {
                GFX.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);
            }
            System.Drawing.Imaging.BitmapData RegionIn_BitmapData = RegionIn_Bitmap.LockBits(new Rectangle(0, 0, RegionIn_Bitmap.Width, RegionIn_Bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            int[] Formatted_Color = new int[3] { Pixel_Color.B, Pixel_Color.G, Pixel_Color.R }; //BGR

            unsafe
            {
                for (int y = 0; y < RegionIn_BitmapData.Height; y++)
                {
                    byte* row = (byte*)RegionIn_BitmapData.Scan0 + (y * RegionIn_BitmapData.Stride);
                    for (int x = 0; x < RegionIn_BitmapData.Width; x++)
                    {
                        if (row[x * 3] >= (Formatted_Color[0] - Shade_Variation) & row[x * 3] <= (Formatted_Color[0] + Shade_Variation)) //青
                            if (row[(x * 3) + 1] >= (Formatted_Color[1] - Shade_Variation) & row[(x * 3) + 1] <= (Formatted_Color[1] + Shade_Variation)) //緑
                                if (row[(x * 3) + 2] >= (Formatted_Color[2] - Shade_Variation) & row[(x * 3) + 2] <= (Formatted_Color[2] + Shade_Variation)) //赤
                                    points.Add(new Point(x + rect.X, y + rect.Y));
                    }
                }
            }
            RegionIn_Bitmap.Dispose();
            return (Point[])points.ToArray(typeof(Point));
        }

        internal static object PixelSearch(Rectangle sourceRectangle, object value, int v)
        {
            throw new NotImplementedException();
        }
    }
}
