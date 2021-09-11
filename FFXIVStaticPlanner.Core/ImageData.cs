using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FFXIVStaticPlanner.Core
{
    public class ImageData
    {
        public string Display
        {
            get;set;
        }

        public ImageSource Source
        {
            get;set;
        }

        public string Name
        {
            get;set;
        }

        public Guid ID
        {
            get;set;
        }

        public string Group
        {
            get;set;
        }

        public void SetRawData ( byte[] rawdata )
        {
            if ( rawdata == null )
            {
                return;
            }

            var bitmap = new BitmapImage();
            using var stream = new MemoryStream(rawdata);

            bitmap.BeginInit ( );
            bitmap.StreamSource = stream;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit ( );

            Source = bitmap;
        }
    }
}
